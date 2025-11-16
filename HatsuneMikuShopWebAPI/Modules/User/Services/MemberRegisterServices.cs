using Common.Helpers;
using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NETCore.MailKit.Core;
using System.Text.RegularExpressions;
using Twilio.Exceptions;
using Twilio.Rest.Verify.V2.Service;
using Twilio.Types;
using Microsoft.Extensions.Logging;

namespace LifetimeLiveHouseWebAPI.Modules.User.Services
{
    public class MemberRegisterServices : IMemberRegisterServices
    {
        private readonly LifetimeLiveHouseSysDBContext _context;
        private readonly IEmailService _emailService;
        private readonly string _frontendBaseUrl;
        private readonly TwilioOptions _twilioOpts;
        private readonly ILogger<MemberRegisterServices>? _logger;

        public MemberRegisterServices(
            LifetimeLiveHouseSysDBContext context,
            IEmailService emailService,
            IConfiguration config,
            IOptions<TwilioOptions> twilioOptions,
            ILogger<MemberRegisterServices>? logger = null)
        {
            _context = context;
            _emailService = emailService;
            _frontendBaseUrl = config["FrontendBaseUrl"] ?? "https://example.com";
            _twilioOpts = twilioOptions.Value;
            _logger = logger;
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
                        EmailVerificationTokenExpiry = DateTime.Now.AddHours(24)

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
                    var emailVerifyLink = $"{_frontendBaseUrl}/verify-email?token={Uri.EscapeDataString(plainToken)}";
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
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return new BadRequestObjectResult("phoneNumber is required.");

            // 1. 標準化國際電話
            string NormalizePhone(string raw)
            {
                // 保留數字與 +
                string cleaned = new string(raw.Where(c => char.IsDigit(c) || c == '+').ToArray());

                // 特殊處理台灣 09xxxxxxxx
                if (cleaned.StartsWith("09") && cleaned.Length >= 10)
                    return "+886" + cleaned.Substring(1);

                // 若是台灣 9xxxxxxxx
                if (cleaned.Length == 9 && cleaned.StartsWith("9"))
                    return "+886" + cleaned;

                // 若是 + 開頭，視為國際格式
                if (cleaned.StartsWith("+"))
                    return cleaned;

                // 若是未加 + 的國際格式，例如 8869xxxxxxx → 加上 +
                if (Regex.IsMatch(cleaned, @"^\d{8,15}$"))
                    return "+" + cleaned;

                return cleaned;
            }

            var normalized = NormalizePhone(phoneNumber);

            // 2. 必須符合 E.164 格式：+國碼 + 最少8碼
            if (!Regex.IsMatch(normalized, @"^\+\d{8,15}$"))
            {
                return new BadRequestObjectResult($"Invalid phone number format: '{normalized}'. Phone must be in E.164 format (e.g., +886912345678).");
            }

            // 3. 檢查帳號是否存在
            var account = await _context.Member
                .AsNoTracking()
                .Where(a => a.CellphoneNumber == normalized || a.CellphoneNumber == phoneNumber)
                .Select(a => new
                {
                    a.MemberID,
                    a.CellphoneNumber,
                    a.MemberPhoneVerificationStatus.IsPhoneVerified
                })
                .FirstOrDefaultAsync();

            if (account == null)
                return new NotFoundObjectResult("此手機號碼尚未註冊");

            // 4. 檢查是否已驗證
            if (account.IsPhoneVerified)
                return new BadRequestObjectResult("此手機號碼已驗證過");

            var serviceSid = _twilioOpts.VerifyServiceSid;

            if (string.IsNullOrWhiteSpace(serviceSid))
                return new StatusCodeResult(500);

            try
            {
                var verification = await VerificationResource.CreateAsync(
                    to: normalized,
                    channel: "sms",
                    pathServiceSid: serviceSid
                );

                return new OkObjectResult(new
                {
                    message = "驗證碼已發送",
                    status = verification.Status,
                    sid = verification.Sid,
                    to = normalized
                });
            }
            catch (ApiException tex)
            {
                return new ObjectResult(new
                {
                    error = "Twilio API error",
                    twilioStatus = tex.Status,
                    twilioCode = tex.Code,
                    message = tex.Message,
                    moreInfo = tex.MoreInfo
                })
                {
                    StatusCode = 502
                };
            }
            catch (Exception ex)
            {
                return new ObjectResult(new
                {
                    error = "伺服器錯誤",
                    message = ex.Message
                })
                {
                    StatusCode = 500
                };
            }
        }

        public async Task<ActionResult<string>> VerifyEmailAsync(string token)
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

        // Verify Phone
        public async Task<ActionResult<string>> VerifyPhoneAsync(long memberId, string code)
        {
            if (memberId <= 0 || string.IsNullOrWhiteSpace(code))
                return new BadRequestObjectResult("memberId and code are required.");

            try
            {
                var account = await _context.Member
                    .AsNoTracking()
                    .Where(a => a.MemberID == memberId)
                    .Select(a => new
                    {
                        a.MemberID,
                        IsPhoneVerified = a.MemberPhoneVerificationStatus.IsPhoneVerified,
                        CellphoneNumber = a.CellphoneNumber
                    })
                    .FirstOrDefaultAsync();

                if (account == null)
                    return new NotFoundObjectResult("帳號不存在");

                if (account.IsPhoneVerified)
                    return new BadRequestObjectResult("此手機號碼已驗證過");

                if (string.IsNullOrWhiteSpace(account.CellphoneNumber))
                    return new BadRequestObjectResult("會員尚未設定手機號碼");

                // 若 DB 內的號碼不是 E.164 格式，視情況先 normalize（可抽成 function）
                string NormalizePhone(string raw)
                {
                    var cleaned = new string(raw.Where(c => char.IsDigit(c) || c == '+').ToArray());
                    if (cleaned.StartsWith("09") && cleaned.Length >= 10)
                        return "+886" + cleaned.Substring(1);
                    if (cleaned.StartsWith("+"))
                        return cleaned;
                    if (Regex.IsMatch(cleaned, @"^\d{8,15}$"))
                        return "+" + cleaned;
                    return cleaned;
                }

                var toNumber = NormalizePhone(account.CellphoneNumber);

                if (!Regex.IsMatch(toNumber, @"^\+\d{8,15}$"))
                    return new BadRequestObjectResult("儲存在資料庫的手機號碼格式錯誤，請聯絡客服或重新輸入手機號碼");

                var serviceSid = _twilioOpts?.VerifyServiceSid;
                if (string.IsNullOrWhiteSpace(serviceSid))
                    return new ObjectResult("VerifyServiceSid 未設定，請檢查系統設定") { StatusCode = 500 };

                VerificationCheckResource verificationCheck;
                try
                {
                    verificationCheck = await VerificationCheckResource.CreateAsync(
                        to: toNumber,
                        code: code,
                        pathServiceSid: serviceSid
                    );
                }
                catch (ApiException tex)
                {
                    _logger?.LogError(tex, "Twilio ApiException during verification check for member {MemberId}", memberId);
                    return new ObjectResult(new
                    {
                        error = "Twilio API error",
                        twilioStatus = tex.Status,
                        twilioCode = tex.Code,
                        message = tex.Message,
                        moreInfo = tex.MoreInfo
                    })
                    { StatusCode = 502 };
                }

                if (verificationCheck == null)
                    return new ObjectResult("未收到 Twilio 回應，請稍後再試") { StatusCode = 502 };

                if (verificationCheck.Status == "approved")
                {
                    await _context.MemberPhoneVerificationStatus
                        .Where(s => s.MemberID == memberId)
                        .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsPhoneVerified, true));

                    return new OkObjectResult("手機驗證成功！");
                }
                else
                {
                    return new BadRequestObjectResult("驗證碼無效或已過期");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error during phone verification for member {MemberId}", memberId);
                return new ObjectResult(new { error = "Internal server error", message = ex.Message }) { StatusCode = 500 };
            }
        }
    }
}


