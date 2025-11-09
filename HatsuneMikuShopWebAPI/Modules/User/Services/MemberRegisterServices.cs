using Common.Helpers;
using Humanizer;
using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NETCore.MailKit.Core;
using Twilio.Rest.Verify.V2.Service;
using Twilio.Types;
namespace LifetimeLiveHouseWebAPI.Modules.User.Services
{
    public class MemberRegisterServices : IMemberRegisterServices
    {
        private readonly LifetimeLiveHouseSysDBContext _context;
        private readonly IEmailService _emailService;
        private readonly string _frontendBaseUrl;
        private readonly TwilioOptions _twilioOpts;

        public MemberRegisterServices(
            LifetimeLiveHouseSysDBContext context,
            IEmailService emailService,
            IConfiguration config,
            IOptions<TwilioOptions> twilioOptions)
        {
            _context = context;
            _emailService = emailService;
            _frontendBaseUrl = config["FrontendBaseUrl"] ?? "https://example.com";
            _twilioOpts = twilioOptions.Value;
        }

        public async Task<ActionResult<string>> RegisterAsync(MemberRegisterDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 檢查信箱／手機是否已被註冊
                if (await _context.MemberAccount.AnyAsync(a => a.Email == dto.Email))
                {
                    return new BadRequestObjectResult("信箱已被註冊");
                }

                else if (await _context.Member.AnyAsync(m => m.CellphoneNumber == dto.CellphoneNumber))
                {
                    return new BadRequestObjectResult("手機號碼已被註冊");
                }
                else {
                    // 新增 Member
                    var member = new Member
                    {
                        Name = dto.Name,
                        Birthday = dto.Birthday,
                        CellphoneNumber = dto.CellphoneNumber
                    };
                    _context.Member.Add(member);
                    await _context.SaveChangesAsync();

                    // 新增 MemberAccount
                    var account = new MemberAccount
                    {
                        MemberID = member.MemberID,
                        Email = dto.Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
                    };
                    _context.MemberAccount.Add(account);
                    await _context.SaveChangesAsync();

                    // 新增 EmailVerificationStatus
                    var emailVer = new MemberEmailVerificationStatus
                    {
                        MemberID = member.MemberID,
                        IsEmailVerified = false,
                        // EmailVerificationTokenHash 留空，稍後更新
                    };
                    _context.MemberEmailVerificationStatus.Add(emailVer);

                    // 新增 PhoneVerificationStatus
                    var phoneVer = new MemberPhoneVerificationStatus
                    {
                        MemberID = member.MemberID,
                        IsPhoneVerified = false
                    };
                    _context.MemberPhoneVerificationStatus.Add(phoneVer);

                    await _context.SaveChangesAsync();

                    // 產生 token 更新 EmailVerificationStatus
                    var plainToken = TokenGeneratorHelper.GeneratePassword(100);
                    emailVer.EmailVerificationTokenHash = BCrypt.Net.BCrypt.HashPassword(plainToken);
                    _context.MemberEmailVerificationStatus.Update(emailVer);

                    await _context.SaveChangesAsync();

                    // 發送驗證信
                    var emailVerifyLink = $"{_frontendBaseUrl}/verify-email?token={Uri.EscapeDataString(plainToken)}&accountId={member.MemberID}";
                    var body = $@"
                    <p>您好 {dto.Name}：</p>
                    <p>請點擊以下連結完成信箱驗證：</p>
                    <p><a href='{emailVerifyLink}'>{emailVerifyLink}</a></p>
                    <p>此連結將在 24 小時後失效。</p>";
                    await _emailService.SendAsync(dto.Email, "會員註冊 – 信箱驗證", body, true);

                    await transaction.CommitAsync();

                    return dto.Name;  // 或回傳成功訊息或 memberId
                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                // 可記錄異常
                throw;
            }
        }
        public async Task<ActionResult<string>> SendVerificationSMSAsync(string phoneNumber)
        {
            var serviceSid = _twilioOpts.VerifyServiceSid;
            var toNumber = new PhoneNumber(phoneNumber); // 確保為 +8869xxxxxxx 格式
            _ = await VerificationResource.CreateAsync(
                to: toNumber.ToString(), // 將 PhoneNumber 物件轉換為字串
                channel: "sms",
                pathServiceSid: serviceSid
            );

            return phoneNumber;
        }
        public async Task<ActionResult<string>> VerifyEmailAsync(long memberId, string token)
        {
            token = Uri.UnescapeDataString(token); // 先解 URI

            // 因為 token 是隨機字串，所以需逐筆比對（BCrypt 雜湊不可逆）
            var validTokens = await _context.MemberEmailVerificationStatus
                .Where(t => t.EmailVerificationTokenExpiry > DateTime.Now)
                .ToListAsync();

            MemberEmailVerificationStatus? account = validTokens.FirstOrDefault(t => BCrypt.Net.BCrypt.Verify(token, t.EmailVerificationTokenHash));

            if (account == null)
                return new BadRequestObjectResult("驗證連結無效");

            account.IsEmailVerified = true;
            account.EmailVerificationTokenHash = null;
            account.EmailVerificationTokenExpiry = null;

            await _context.SaveChangesAsync();
            return new OkObjectResult("信箱驗證成功！");
        }

        public async Task<ActionResult<string>> VerifyPhoneAsync(long memberId, string code)
        {
            var account = await _context.Member
                .AsNoTracking()
                    .Where(a => a.MemberID == memberId)
                    .Select(a => new
                    {
                        a.MemberID,
                        a.MemberPhoneVerificationStatus.IsPhoneVerified,
                        a.CellphoneNumber,
                    })
                    .FirstOrDefaultAsync();

            if (account == null)
                return new NotFoundObjectResult("帳號不存在");

            var serviceSid = _twilioOpts.VerifyServiceSid;
            var toNumber = new PhoneNumber(account.CellphoneNumber);

            var verificationCheck = await VerificationCheckResource.CreateAsync(
                to: toNumber.ToString(),
                code: code,
                pathServiceSid: serviceSid
            );

            if (verificationCheck.Status == "approved")
            {
                await _context.MemberPhoneVerificationStatus
                    .Where(s => s.MemberID == memberId)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(p => p.IsPhoneVerified, true));

                return new OkObjectResult("手機驗證成功！");
            }
            else
            {
                return new BadRequestObjectResult("驗證碼無效或已過期");
            }
        }
    }
}


