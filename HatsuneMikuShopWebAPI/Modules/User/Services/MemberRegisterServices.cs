using Common.Helpers;
using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NETCore.MailKit.Core;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Twilio.Exceptions;
using Twilio.Rest.Verify.V2.Service;

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
            try
            {
                // 1. 檢查信箱是否已被註冊 (使用 AsNoTracking 提升查詢效能)
                if (await _context.MemberAccount.AsNoTracking().AnyAsync(a => a.Email == dto.Email))
                {
                    return new BadRequestObjectResult("信箱已被註冊");
                }

                // 2. 密碼使用 BCrypt 雜湊 (放進 Task.Run 避免阻塞主執行緒)
                var passwordHash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(dto.Password, 10));

                // 3. Email Token 改用 SHA256 雜湊 (運算極快，不到 1 毫秒)
                var plainTokenString = TokenGeneratorHelper.GeneratePassword(100);
                var tokenHash = HashStringSHA256(plainTokenString);

                // 4. 新增 Member (第 1 次資料庫寫入，為了取得自動遞增的 MemberID)
                var member = new Member
                {
                    Name = dto.Name,
                    Birthday = dto.Birthday
                };
                _context.Member.Add(member);
                await _context.SaveChangesAsync();

                // 5. 批次新增所有關聯資料
                var account = new MemberAccount
                {
                    MemberID = member.MemberID,
                    Email = dto.Email,
                    Password = passwordHash
                };
                _context.MemberAccount.Add(account);

                var emailVer = new MemberEmailVerificationStatus
                {
                    MemberID = member.MemberID,
                    IsEmailVerified = false,
                    EmailVerificationTokenExpiry = DateTime.Now.AddHours(24),
                    EmailVerificationTokenHash = tokenHash // 存入 SHA256 Hash
                };
                _context.MemberEmailVerificationStatus.Add(emailVer);

                var phoneVer = new MemberPhoneVerificationStatus
                {
                    MemberID = member.MemberID,
                    IsPhoneVerified = false
                };
                _context.MemberPhoneVerificationStatus.Add(phoneVer);

                // 第 2 次資料庫寫入 (完成所有關聯綁定)
                await _context.SaveChangesAsync();

                // 6. 組合帶有 MemberID 的 Token (解決驗證時的效能災難)
                var combinedToken = $"{member.MemberID}:{plainTokenString}";
                var emailVerifyLink = $"{_frontendBaseUrl}/verify-email?token={Uri.EscapeDataString(combinedToken)}";

                var body = $@"
                <p>您好 {dto.Name}：</p>
                <p>請點擊以下連結完成信箱驗證：</p>
                <p><a href='{emailVerifyLink}'>{emailVerifyLink}</a></p>
                <p>此連結將在 24 小時後失效。</p>";

                // 7. 背景發送 Email (Fire-and-forget)，讓前端瞬間收到「註冊成功」回應，不用等 SMTP 伺服器
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _emailService.SendAsync(dto.Email, "會員註冊 – 信箱驗證", body, true);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Failed to send registration email to {Email}", dto.Email);
                    }
                });

                return dto.Name;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "RegisterAsync Error");
                // 在正式環境建議不要直接把 ex.ToString() 傳給前端，這裡保留方便你開發除錯
                return new BadRequestObjectResult(ex.ToString());
            }
        }

        public async Task<ActionResult<string>> VerifyEmailAsync(string token)
        {
            token = Uri.UnescapeDataString(token);

            // 1. 拆解 Token，取得 MemberID (格式：MemberID:RandomToken)
            var parts = token.Split(':');
            if (parts.Length != 2 || !long.TryParse(parts[0], out long memberId))
            {
                return new BadRequestObjectResult("無效的驗證連結格式");
            }

            var plainTokenString = parts[1];

            // 2. 利用 MemberID 精準抓取單一筆資料 (時間複雜度 O(1))
            var account = await _context.MemberEmailVerificationStatus
                .FirstOrDefaultAsync(t => t.MemberID == memberId && t.EmailVerificationTokenExpiry > DateTime.Now);

            if (account == null)
            {
                return new BadRequestObjectResult("驗證連結無效或已過期");
            }

            // 3. 將前端傳來的 token 再次進行 SHA256 雜湊比對
            var expectedHash = HashStringSHA256(plainTokenString);
            if (account.EmailVerificationTokenHash != expectedHash)
            {
                return new BadRequestObjectResult("驗證連結無效");
            }

            // 4. 驗證成功，更新狀態
            account.IsEmailVerified = true;
            account.EmailVerificationTokenHash = null;
            account.EmailVerificationTokenExpiry = null;

            await _context.SaveChangesAsync();
            return new OkObjectResult("信箱驗證成功！");
        }

        public async Task<ActionResult<string>> SendVerificationSMSAsync(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return new BadRequestObjectResult("phoneNumber is required.");

            var normalized = NormalizePhoneNumber(phoneNumber);

            // 必須符合 E.164 格式
            if (!Regex.IsMatch(normalized, @"^\+\d{8,15}$"))
            {
                return new BadRequestObjectResult($"Invalid phone number format: '{normalized}'.");
            }

            // 檢查帳號與驗證狀態
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
                _logger?.LogError(tex, "Twilio API error");
                return new ObjectResult(new
                {
                    error = "Twilio API error",
                    twilioStatus = tex.Status,
                    message = tex.Message
                })
                { StatusCode = 502 };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "伺服器錯誤");
                return new ObjectResult(new { error = "伺服器錯誤", message = ex.Message }) { StatusCode = 500 };
            }
        }

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
                        a.MemberPhoneVerificationStatus.IsPhoneVerified,
                        a.CellphoneNumber
                    })
                    .FirstOrDefaultAsync();

                if (account == null)
                    return new NotFoundObjectResult("帳號不存在");

                if (account.IsPhoneVerified)
                    return new BadRequestObjectResult("此手機號碼已驗證過");

                if (string.IsNullOrWhiteSpace(account.CellphoneNumber))
                    return new BadRequestObjectResult("會員尚未設定手機號碼");

                var toNumber = NormalizePhoneNumber(account.CellphoneNumber);

                if (!Regex.IsMatch(toNumber, @"^\+\d{8,15}$"))
                    return new BadRequestObjectResult("儲存在資料庫的手機號碼格式錯誤");

                var serviceSid = _twilioOpts?.VerifyServiceSid;
                if (string.IsNullOrWhiteSpace(serviceSid))
                    return new ObjectResult("VerifyServiceSid 未設定") { StatusCode = 500 };

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
                    _logger?.LogError(tex, "Twilio ApiException");
                    return new ObjectResult(new { error = "Twilio API error", message = tex.Message }) { StatusCode = 502 };
                }

                if (verificationCheck == null)
                    return new ObjectResult("未收到 Twilio 回應，請稍後再試") { StatusCode = 502 };

                if (verificationCheck.Status == "approved")
                {
                    // 使用 ExecuteUpdateAsync 直接在 DB 執行 Update，效能極佳
                    await _context.MemberPhoneVerificationStatus
                        .Where(s => s.MemberID == memberId)
                        .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsPhoneVerified, true));

                    return new OkObjectResult("手機驗證成功！");
                }

                return new BadRequestObjectResult("驗證碼無效或已過期");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error during phone verification");
                return new ObjectResult(new { error = "Internal server error", message = ex.Message }) { StatusCode = 500 };
            }
        }

        // ================= 工具方法 =================

        // 極速 SHA256 雜湊 (取代用在 Token 的 BCrypt)
        private static string HashStringSHA256(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }

        // 正規化電話號碼
        private static string NormalizePhoneNumber(string raw)
        {
            string cleaned = new string(raw.Where(c => char.IsDigit(c) || c == '+').ToArray());
            if (cleaned.StartsWith("09") && cleaned.Length >= 10) return "+886" + cleaned.Substring(1);
            if (cleaned.Length == 9 && cleaned.StartsWith("9")) return "+886" + cleaned;
            if (cleaned.StartsWith("+")) return cleaned;
            if (Regex.IsMatch(cleaned, @"^\d{8,15}$")) return "+" + cleaned;
            return cleaned;
        }
    }
}