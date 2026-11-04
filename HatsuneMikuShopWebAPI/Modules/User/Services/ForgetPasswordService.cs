using Common.Helpers;
using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Cryptography;
using System.Text;

namespace LifetimeLiveHouseWebAPI.Modules.User.Services
{
    public class ForgetPasswordService(
            LifetimeLiveHouseSysDBContext context,
            IServiceScopeFactory scopeFactory,
            IConfiguration config,
            IWebHostEnvironment env,
            IDistributedCache cache, // 💡 注入 Redis 快取
            ILogger<ForgetPasswordService>? logger = null) : IForgetPasswordService
    {
        private readonly LifetimeLiveHouseSysDBContext _context = context;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly IWebHostEnvironment _env = env;
        private readonly IDistributedCache _cache = cache;
        private readonly ILogger<ForgetPasswordService>? _logger = logger;
        private readonly string _frontendBaseUrl = config["FrontendBaseUrl"] ?? "https://livetimelivehouse.sakuyaonline.uk";

        public async Task<string> SendForgotPasswordEmailAsync(ForgotPasswordDto dto)
        {
            // 💡 Redis 防刷機制：檢查該信箱是否在 60 秒內已經發送過
            var rateLimitKey = $"ForgetPwd_RateLimit_{dto.Email}";
            var isLocked = await _cache.GetStringAsync(rateLimitKey);
            if (!string.IsNullOrEmpty(isLocked))
                throw new InvalidOperationException("發送過於頻繁，請於 60 秒後再試。");

            var user = await _context.MemberAccount.SingleOrDefaultAsync(u => u.Email == dto.Email);
            var responseMsg = "如果該信箱有註冊，我們已發送重設密碼信件，請檢查信件，若未收到郵件請檢查您的垃圾信件夾。";

            if (user != null)
            {
                var plainToken = TokenGeneratorHelper.GeneratePassword(100);
                var prt = new PasswordResetToken
                {
                    MemberID = user.MemberID,
                    TokenHash = ComputeSha256Hash(plainToken),
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddHours(1),
                    Used = false
                };

                _context.PasswordResetToken.Add(prt);
                await _context.SaveChangesAsync();

                string resetLink = $"{_frontendBaseUrl}/reset-password?token={Uri.EscapeDataString(plainToken)}";
                var body = $"請在1小時內點擊以下連結以重設您的密碼：<br/><a href=\"{resetLink}\">{resetLink}</a>";

                if (_env.IsDevelopment())
                {
                    using var scope = _scopeFactory.CreateScope();
                    var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
                    await emailService.SendEmailAsync(user.Email, "重設密碼通知", body);
                }
                else
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            using var scope = _scopeFactory.CreateScope();
                            var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
                            await emailService.SendEmailAsync(user.Email, "重設密碼通知", body);
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "發送重設密碼信件失敗 {Email}", user.Email);
                        }
                    });
                }
            }

            // 💡 成功執行後，在 Redis 寫入鎖定標記，維持 60 秒
            await _cache.SetStringAsync(rateLimitKey, "1", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
            });

            return responseMsg;
        }

        public async Task<string> ValidResetPasswordTokenAsync(ValidResetPasswordTokenDto dto)
        {
            dto.InputToken = Uri.UnescapeDataString(dto.InputToken);
            var inputHash = ComputeSha256Hash(dto.InputToken);

            // 優化：利用 SHA256 讓資料庫直接進行 O(1) 篩選，不再將所有 Token 拉回記憶體
            var isValid = await _context.PasswordResetToken
                .AnyAsync(t => t.TokenHash == inputHash && !t.Used && t.ExpiresAt > DateTime.UtcNow);

            if (!isValid)
                throw new InvalidOperationException("驗證連結無效或已過期。");

            return "Token 有效";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto dto)
        {
            dto.InputToken = Uri.UnescapeDataString(dto.InputToken);

            if (dto.NewPassword != dto.ConfirmPassword)
                throw new InvalidOperationException("密碼與確認密碼不一致。");

            var inputHash = ComputeSha256Hash(dto.InputToken);

            // 優化：直接從資料庫精準鎖定該筆 Token
            var prt = await _context.PasswordResetToken
                .FirstOrDefaultAsync(t => t.TokenHash == inputHash && !t.Used && t.ExpiresAt > DateTime.UtcNow);

            if (prt == null)
                throw new InvalidOperationException("驗證連結無效或已過期。");

            var user = await _context.MemberAccount.FirstOrDefaultAsync(a => a.MemberID == prt.MemberID);
            if (user == null)
                throw new InvalidOperationException("使用者不存在。");

            // 使用者的登入密碼維持使用 BCrypt 進行高強度雜湊
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            prt.Used = true;
            prt.UsedAt = DateTime.UtcNow; // 修正為 UTC

            await _context.SaveChangesAsync();
            return "密碼已重設成功。";
        }

        public async Task CleanupExpiredTokensAsync()
        {
            await _context.PasswordResetToken
                .Where(t => t.ExpiresAt < DateTime.UtcNow || t.Used) // 修正為 UTC
                .ExecuteDeleteAsync();
        }

        // 獨立的 SHA256 雜湊輔助方法 (專門用於一次性 Token)
        private static string ComputeSha256Hash(string rawData)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawData));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}