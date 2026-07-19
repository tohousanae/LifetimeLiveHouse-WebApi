using Common.Helpers;
using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    // 必須加上 partial 以支援 GeneratedRegex
    public partial class MemberRegisterServices : IMemberRegisterServices
    {
        private readonly LifetimeLiveHouseSysDBContext _context;
        private readonly string _frontendBaseUrl;
        private readonly TwilioOptions _twilioOpts;
        private readonly ILogger<MemberRegisterServices>? _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        // 優化：正規表達式源碼生成 (編譯期預先編譯，消滅執行期解析 CPU 耗時)
        [GeneratedRegex(@"^\+\d{8,15}$", RegexOptions.Compiled)]
        private static partial Regex PhoneNumberRegex();

        public MemberRegisterServices(
            LifetimeLiveHouseSysDBContext context,
            IConfiguration config,
            IOptions<TwilioOptions> twilioOptions,
            IServiceScopeFactory scopeFactory,
            ILogger<MemberRegisterServices>? logger = null)
        {
            _context = context;
            _frontendBaseUrl = config["FrontendBaseUrl"] ?? "https://example.com";
            _twilioOpts = twilioOptions.Value;
            _scopeFactory = scopeFactory; // 取代直接注入 scoped email service
            _logger = logger;
        }

        public async Task<ActionResult<string>> RegisterAsync(MemberRegisterDTO dto)
        {
            try
            {
                // 1. 檢查信箱是否已被註冊 (AsNoTracking 提升查詢效能)[cite: 1]
                if (await _context.MemberAccount.AsNoTracking().AnyAsync(a => a.Email == dto.Email))
                {
                    return new BadRequestObjectResult("信箱已被註冊");
                }

                // 2. 密碼使用 BCrypt 雜湊 (放進 Task.Run 避免阻塞主執行緒)[cite: 1]
                var passwordHash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(dto.Password, 10));

                // 3. 產生 Token 並雜湊
                var plainTokenString = TokenGeneratorHelper.GeneratePassword(100);
                var tokenHash = HashStringSHA256(plainTokenString);

                // 4. 優化：反向物件圖寫入 (從 MemberAccount 往下建立 Member，零修改 Model)
                // 這樣可以讓 EF Core 在單一次 SaveChangesAsync (一次 Transaction) 中完成所有資料表的寫入
                var account = new MemberAccount
                {
                    Email = dto.Email,
                    Password = passwordHash,
                    Member = new Member
                    {
                        Name = dto.Name,
                        Birthday = dto.Birthday,

                        // 直接掛載實體模型的導覽屬性[cite: 2]
                        MemberEmailVerificationStatus = new MemberEmailVerificationStatus
                        {
                            IsEmailVerified = false,
                            EmailVerificationTokenExpiry = DateTime.Now.AddHours(24),
                            EmailVerificationTokenHash = tokenHash
                        },
                        MemberPhoneVerificationStatus = new MemberPhoneVerificationStatus
                        {
                            IsPhoneVerified = false
                        }
                    }
                };

                // 5. 執行單次寫入，大幅降低資料庫 I/O 延遲
                _context.MemberAccount.Add(account);
                await _context.SaveChangesAsync();

                // 從剛寫入完成的實體中直接取得 EF Core 填入的自動遞增 ID[cite: 3]
                var combinedToken = $"{account.Member.MemberID}:{plainTokenString}";
                var emailVerifyLink = $"{_frontendBaseUrl}/verify-email?token={Uri.EscapeDataString(combinedToken)}";

                var body = $@"
                <p>您好 {dto.Name}：</p>
                <p>請點擊以下連結完成信箱驗證：</p>
                <p><a href='{emailVerifyLink}'>{emailVerifyLink}</a></p>
                <p>此連結將在 24 小時後失效。</p>";

                // 6. 優化：安全的背景發信任務
                // 使用 _scopeFactory 建立獨立 Scope，避免 Request 結束導致 Service 被 Dispose
                _ = Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                        await emailService.SendAsync(dto.Email, "會員註冊 – 信箱驗證", body, true);
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
                return new BadRequestObjectResult("伺服器發生錯誤");
            }
        }

        public async Task<ActionResult<string>> VerifyEmailAsync(string token)
        {
            token = Uri.UnescapeDataString(token);
            var parts = token.Split(':');

            if (parts.Length != 2 || !long.TryParse(parts[0], out long memberId))
            {
                return new BadRequestObjectResult("無效的驗證連結格式");
            }

            var expectedHash = HashStringSHA256(parts[1]);

            // 優化：直接在資料庫端執行驗證與更新，消滅 SELECT 至應用程式的 I/O 成本與記憶體分配[cite: 1]
            var rowsAffected = await _context.MemberEmailVerificationStatus
                .Where(t => t.MemberID == memberId
                         && t.EmailVerificationTokenExpiry > DateTime.Now
                         && t.EmailVerificationTokenHash == expectedHash)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.IsEmailVerified, true)
                    .SetProperty(p => p.EmailVerificationTokenHash, (string?)null)
                    .SetProperty(p => p.EmailVerificationTokenExpiry, (DateTime?)null));

            if (rowsAffected == 0)
            {
                return new BadRequestObjectResult("驗證連結無效或已過期");
            }

            return new OkObjectResult("信箱驗證成功！");
        }

        public async Task<ActionResult<string>> SendVerificationSMSAsync(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return new BadRequestObjectResult("phoneNumber is required.");

            var normalized = NormalizePhoneNumber(phoneNumber);

            if (!PhoneNumberRegex().IsMatch(normalized))
            {
                return new BadRequestObjectResult($"Invalid phone number format: '{normalized}'.");
            }

            var account = await _context.Member
                .AsNoTracking()
                .Where(a => a.CellphoneNumber == normalized || a.CellphoneNumber == phoneNumber)
                .Select(a => new
                {
                    // 根據 Member 模型的關聯抓取驗證狀態[cite: 2]
                    IsPhoneVerified = a.MemberPhoneVerificationStatus != null && a.MemberPhoneVerificationStatus.IsPhoneVerified
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
                return new ObjectResult(new { error = "Twilio API error", message = tex.Message }) { StatusCode = 502 };
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
                        IsPhoneVerified = a.MemberPhoneVerificationStatus != null && a.MemberPhoneVerificationStatus.IsPhoneVerified,
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

                if (!PhoneNumberRegex().IsMatch(toNumber))
                    return new BadRequestObjectResult("儲存在資料庫的手機號碼格式錯誤");

                var serviceSid = _twilioOpts?.VerifyServiceSid;
                if (string.IsNullOrWhiteSpace(serviceSid))
                    return new ObjectResult("VerifyServiceSid 未設定") { StatusCode = 500 };

                var verificationCheck = await VerificationCheckResource.CreateAsync(
                    to: toNumber,
                    code: code,
                    pathServiceSid: serviceSid
                );

                if (verificationCheck?.Status == "approved")
                {
                    // 優化：直接使用 ExecuteUpdateAsync 在 DB 執行 Update，效能極佳[cite: 1]
                    await _context.MemberPhoneVerificationStatus
                        .Where(s => s.MemberID == memberId)
                        .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsPhoneVerified, true));

                    return new OkObjectResult("手機驗證成功！");
                }

                return new BadRequestObjectResult("驗證碼無效或已過期");
            }
            catch (ApiException tex)
            {
                _logger?.LogError(tex, "Twilio ApiException");
                return new ObjectResult(new { error = "Twilio API error", message = tex.Message }) { StatusCode = 502 };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error during phone verification");
                return new ObjectResult(new { error = "Internal server error" }) { StatusCode = 500 };
            }
        }

        // ================= 工具方法 =================

        // 優化：使用現代 .NET 的 HashData，無須實例化 SHA256 即可進行極速運算，免除記憶體配置
        private static string HashStringSHA256(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }

        // 優化：利用 stackalloc 消除 LINQ 產生的陣列與字串垃圾 (降低 GC 回收頻率)
        private static string NormalizePhoneNumber(string raw)
        {
            // 在堆疊上宣告緩衝區，取代原本的 new string() 與 ToArray()
            Span<char> buffer = stackalloc char[raw.Length];
            int length = 0;

            foreach (char c in raw)
            {
                if (char.IsDigit(c) || c == '+')
                {
                    buffer[length++] = c;
                }
            }

            var cleaned = new string(buffer[..length]);

            if (cleaned.StartsWith("09") && cleaned.Length >= 10) return "+886" + cleaned.Substring(1);
            if (cleaned.Length == 9 && cleaned.StartsWith("9")) return "+886" + cleaned;
            if (cleaned.StartsWith("+")) return cleaned;
            if (PhoneNumberRegex().IsMatch(cleaned)) return "+" + cleaned;

            return cleaned;
        }
    }
}