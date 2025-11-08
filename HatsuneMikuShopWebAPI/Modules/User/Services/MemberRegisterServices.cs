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

        public async Task<ActionResult<string>> MemberRegisterAsync(MemberRegisterDTO dto)
        {
            // 檢查信箱／手機是否已被註冊
            var checkResult = await CheckEmailOrCellphoneAlreadyRegisteredAsync(dto.Email, dto.CellphoneNumber);
            if (checkResult != null)
            {
                return checkResult;
            }

            // 建立會員、會員帳號、未驗證的會員手機信箱驗證狀態資料，並發出驗證信
            await InsertMemberAsync(dto);

            // 發送手機簡訊驗證（使用 Twilio Verify）
            await SendVerificationSMSAsync(dto.CellphoneNumber);
            return new OkObjectResult($"註冊成功，我們已發送驗證信至您的信箱({dto.Email})，請點選信件中的連結完成驗證以啟用帳號)");
        }
        public async Task<ActionResult<string>> SendVerificationEmailAsync(string memberName, string email, long memberID)
        {
            // 將token存入會員信箱驗證資料表
            // 用memberID找出對應的MemberEmailVerificationStatus並更新token和過期時間
            var u = await _context.MemberEmailVerificationStatus.FindAsync(memberID);
            if (u == null)
            {
                // 在 service 類別中應使用 ActionResult 的具體結果型別，不可呼叫 ControllerBase 的 helper 方法（例如 NotFound()）
                return new NotFoundObjectResult("查無資料");
            }

            // 用memberID找出對應的MemberEmailVerificationStatus並更新token和過期時間
            var plainToken = TokenGeneratorHelper.GeneratePassword(100);
            var prt = new MemberEmailVerificationStatus
            {
                MemberID = u.MemberID,
                EmailVerificationTokenHash = BCrypt.Net.BCrypt.HashPassword(plainToken),
            };
            _context.MemberEmailVerificationStatus.Add(prt);
            await _context.SaveChangesAsync();

            var emailVerifyLink = $"{_frontendBaseUrl}/verify-email?token={Uri.EscapeDataString(plainToken)}&accountId={memberID}";
            var emailBody = $@"
                <p>您好 {memberName}：</p>
                <p>請點擊以下連結完成信箱驗證：</p>
                <p><a href='{emailVerifyLink}'>點我完成驗證</a></p>
                <p>此連結將在 24 小時後失效。</p>";
            await _emailService.SendAsync(email, "會員註冊 – 信箱驗證", emailBody, true);

            return memberName;
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
        public async Task<ActionResult<string>?> CheckEmailOrCellphoneAlreadyRegisteredAsync(string email, string cellphoneNumber)
        {
            if (await _context.MemberAccount.AnyAsync(u => u.Email == email))
            {
                return new BadRequestObjectResult("信箱已被註冊");
            }

            if (await _context.Member.AnyAsync(u => u.CellphoneNumber == cellphoneNumber))
            {
                return new BadRequestObjectResult("手機號碼已被註冊");
            }

            return null;  // 表示都沒問題
        }
        public async Task<Member> InsertMemberAsync(MemberRegisterDTO dto)
        {
            var member = new Member
            {
                Name = dto.Name,
                Birthday = dto.Birthday,
                CellphoneNumber = dto.CellphoneNumber
            };

            _context.Member.Add(member);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            // 新增帳號並回傳 Member（或回傳帳號，看你的需求）
            await InsertMemberAccountAsync(member.MemberID, dto.Email, dto.Password);
            await InsertMemberEmailVerificationStatusAsync(member.MemberID);
            await InsertMemberPhoneVerificationStatusAsync(member.MemberID);

            // 發送信箱驗證信
            await SendVerificationEmailAsync(dto.Name, dto.Email, member.MemberID);

            return member;
        }

        public async Task<MemberAccount> InsertMemberAccountAsync(long memberId, string email, string password)
        {
            var account = new MemberAccount
            {
                MemberID = memberId,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                // 你看是否還有其他欄位要設定，例如 IsVerified = false
            };

            _context.MemberAccount.Add(account);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return account;
        }
        public async Task<MemberEmailVerificationStatus> InsertMemberEmailVerificationStatusAsync(long memberId)
        {
            var verificationStatus = new MemberEmailVerificationStatus
            {
                MemberID = memberId,
                IsEmailVerified = false,
            };

            _context.MemberEmailVerificationStatus.Add(verificationStatus);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            return verificationStatus;
        }
        public async Task<MemberPhoneVerificationStatus> InsertMemberPhoneVerificationStatusAsync(long memberId)
        {
            var verificationStatus = new MemberPhoneVerificationStatus
            {
                MemberID = memberId,
                IsPhoneVerified = false
            };

            _context.MemberPhoneVerificationStatus.Add(verificationStatus);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            return verificationStatus;
        }
        public async Task<ActionResult<string>> VerifyEmailAsync(long memberId, string token)
        {
            dto.InputToken = Uri.UnescapeDataString(dto.InputToken); // 先解 URI

            // 因為 token 是隨機字串，所以需逐筆比對（BCrypt 雜湊不可逆）
            var validTokens = await _context.PasswordResetToken
                .Where(t => !t.Used && t.ExpiresAt > DateTime.Now)
                .ToListAsync();

            PasswordResetToken? prt = validTokens.FirstOrDefault(t => BCrypt.Net.BCrypt.Verify(dto.InputToken, t.TokenHash));

            if (account == null)
                return new BadRequestObjectResult("驗證連結無效");

            if (account.MemberEmailVerificationStatus.EmailVerificationTokenExpiry < DateTime.Now)
                return new BadRequestObjectResult("驗證連結已過期");

            account.MemberEmailVerificationStatus.IsEmailVerified = true;
            account.MemberEmailVerificationStatus.EmailVerificationTokenHash = null;
            account.MemberEmailVerificationStatus.EmailVerificationTokenExpiry = null;

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


