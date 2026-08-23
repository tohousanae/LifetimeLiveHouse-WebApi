//using LifetimeLiveHouse.Access.Data;
//using LifetimeLiveHouse.Models;
//using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using System.Security.Cryptography;
//using System.Text;
//using System.Text.RegularExpressions;
//using Twilio.Exceptions;
//using Twilio.Rest.Verify.V2.Service;

//namespace LifetimeLiveHouseWebAPI.Modules.User.Services
//{
//    // 必須加上 partial 以支援 GeneratedRegex
//    public partial class MemberVerificationService(
//        LifetimeLiveHouseSysDBContext context,
//        IOptions<TwilioOptions> twilioOptions,
//        ILogger<MemberVerificationService>? logger = null) : IMemberVerificationService
//    {
//        private readonly LifetimeLiveHouseSysDBContext _context = context;
//        private readonly TwilioOptions _twilioOpts = twilioOptions.Value;
//        private readonly ILogger<MemberVerificationService>? _logger = logger;

//        // 優化：正規表達式源碼生成 (編譯期預先編譯，消滅執行期解析 CPU 耗時)
//        [GeneratedRegex(@"^\+\d{8,15}$", RegexOptions.Compiled)]
//        private static partial Regex PhoneNumberRegex();

//        public async Task<ActionResult<object>> VerifyEmailAsync(string token)
//        {
//            token = Uri.UnescapeDataString(token);
//            var parts = token.Split(':');

//            if (parts.Length < 2 || !long.TryParse(parts[0], out long memberId))
//            {
//                return new BadRequestObjectResult("無效的驗證連結格式");
//            }

//            var expectedHash = HashStringSHA256(parts[1]);
//            string redirectUrl = "/"; // 預設跳轉回首頁

//            // 解析第三段的跳轉路徑 (智慧跳轉)
//            if (parts.Length >= 3)
//            {
//                try
//                {
//                    redirectUrl = Encoding.UTF8.GetString(Convert.FromBase64String(parts[2]));
//                }
//                catch
//                {
//                    /* 解析失敗則忽略，保留預設跳轉首頁 */
//                }
//            }

//            // 優化：直接在資料庫端執行驗證與更新，消滅 SELECT 至應用程式的 I/O 成本與記憶體分配
//            var rowsAffected = await _context.MemberEmailVerificationStatus
//                .Where(t => t.MemberID == memberId
//                         && t.EmailVerificationTokenExpiry > DateTime.Now
//                         && t.EmailVerificationTokenHash == expectedHash)
//                .ExecuteUpdateAsync(s => s
//                    .SetProperty(p => p.IsEmailVerified, true)
//                    .SetProperty(p => p.EmailVerificationTokenHash, (string?)null)
//                    .SetProperty(p => p.EmailVerificationTokenExpiry, (DateTime?)null));

//            if (rowsAffected == 0)
//            {
//                return new BadRequestObjectResult("驗證連結無效或已過期");
//            }

//            // 回傳成功訊息與目標路徑，讓前端 Vue 接收 JSON 並自動跳轉
//            return new OkObjectResult(new { message = "信箱驗證成功！", redirectUrl });
//        }

//        public async Task<ActionResult<string>> SendVerificationSMSAsync(string phoneNumber)
//        {
//            if (string.IsNullOrWhiteSpace(phoneNumber))
//                return new BadRequestObjectResult("phoneNumber is required.");

//            var normalized = NormalizePhoneNumber(phoneNumber);

//            if (!PhoneNumberRegex().IsMatch(normalized))
//            {
//                return new BadRequestObjectResult($"Invalid phone number format: '{normalized}'.");
//            }

//            var account = await _context.Member
//                .AsNoTracking()
//                .Where(a => a.CellphoneNumber == normalized || a.CellphoneNumber == phoneNumber)
//                .Select(a => new
//                {
//                    // 根據 Member 模型的關聯抓取驗證狀態
//                    IsPhoneVerified = a.MemberPhoneVerificationStatus != null && a.MemberPhoneVerificationStatus.IsPhoneVerified
//                })
//                .FirstOrDefaultAsync();

//            if (account == null)
//                return new NotFoundObjectResult("此手機號碼尚未註冊");

//            if (account.IsPhoneVerified)
//                return new BadRequestObjectResult("此手機號碼已驗證過");

//            var serviceSid = _twilioOpts.VerifyServiceSid;
//            if (string.IsNullOrWhiteSpace(serviceSid))
//                return new StatusCodeResult(500);

//            try
//            {
//                var verification = await VerificationResource.CreateAsync(
//                    to: normalized,
//                    channel: "sms",
//                    pathServiceSid: serviceSid
//                );

//                return new OkObjectResult(new
//                {
//                    message = "驗證碼已發送",
//                    status = verification.Status,
//                    sid = verification.Sid,
//                    to = normalized
//                });
//            }
//            catch (ApiException tex)
//            {
//                _logger?.LogError(tex, "Twilio API error");
//                return new ObjectResult(new { error = "Twilio API error", message = tex.Message }) { StatusCode = 502 };
//            }
//        }

//        public async Task<ActionResult<string>> VerifyPhoneAsync(long memberId, string code)
//        {
//            if (memberId <= 0 || string.IsNullOrWhiteSpace(code))
//                return new BadRequestObjectResult("memberId and code are required.");

//            try
//            {
//                var account = await _context.Member
//                    .AsNoTracking()
//                    .Where(a => a.MemberID == memberId)
//                    .Select(a => new
//                    {
//                        IsPhoneVerified = a.MemberPhoneVerificationStatus != null && a.MemberPhoneVerificationStatus.IsPhoneVerified,
//                        a.CellphoneNumber
//                    })
//                    .FirstOrDefaultAsync();

//                if (account == null)
//                    return new NotFoundObjectResult("帳號不存在");

//                if (account.IsPhoneVerified)
//                    return new BadRequestObjectResult("此手機號碼已驗證過");

//                if (string.IsNullOrWhiteSpace(account.CellphoneNumber))
//                    return new BadRequestObjectResult("會員尚未設定手機號碼");

//                var toNumber = NormalizePhoneNumber(account.CellphoneNumber);

//                if (!PhoneNumberRegex().IsMatch(toNumber))
//                    return new BadRequestObjectResult("儲存在資料庫的手機號碼格式錯誤");

//                var serviceSid = _twilioOpts?.VerifyServiceSid;
//                if (string.IsNullOrWhiteSpace(serviceSid))
//                    return new ObjectResult("VerifyServiceSid 未設定") { StatusCode = 500 };

//                var verificationCheck = await VerificationCheckResource.CreateAsync(
//                    to: toNumber,
//                    code: code,
//                    pathServiceSid: serviceSid
//                );

//                if (verificationCheck?.Status == "approved")
//                {
//                    // 優化：直接使用 ExecuteUpdateAsync 在 DB 執行 Update，效能極佳
//                    await _context.MemberPhoneVerificationStatus
//                        .Where(s => s.MemberID == memberId)
//                        .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsPhoneVerified, true));

//                    return new OkObjectResult("手機驗證成功！");
//                }

//                return new BadRequestObjectResult("驗證碼無效或已過期");
//            }
//            catch (ApiException tex)
//            {
//                _logger?.LogError(tex, "Twilio ApiException");
//                return new ObjectResult(new { error = "Twilio API error", message = tex.Message }) { StatusCode = 502 };
//            }
//            catch (Exception ex)
//            {
//                _logger?.LogError(ex, "Unexpected error during phone verification");
//                return new ObjectResult(new { error = "Internal server error" }) { StatusCode = 500 };
//            }
//        }

//        // ================= 工具方法 =================

//        // 優化：使用現代 .NET 的 HashData，無須實例化 SHA256 即可進行極速運算，免除記憶體配置
//        private static string HashStringSHA256(string input)
//        {
//            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
//            return Convert.ToBase64String(bytes);
//        }

//        // 優化：利用 stackalloc 消除 LINQ 產生的陣列與字串垃圾 (降低 GC 回收頻率)
//        private static string NormalizePhoneNumber(string raw)
//        {
//            Span<char> buffer = stackalloc char[raw.Length];
//            int length = 0;

//            foreach (char c in raw)
//            {
//                if (char.IsDigit(c) || c == '+')
//                {
//                    buffer[length++] = c;
//                }
//            }

//            var cleaned = new string(buffer[..length]);

//            if (cleaned.StartsWith("09") && cleaned.Length >= 10) return "+886" + cleaned.Substring(1);
//            if (cleaned.Length == 9 && cleaned.StartsWith("9")) return "+886" + cleaned;
//            if (cleaned.StartsWith("+")) return cleaned;
//            if (PhoneNumberRegex().IsMatch(cleaned)) return "+" + cleaned;

//            return cleaned;
//        }
//    }
//}