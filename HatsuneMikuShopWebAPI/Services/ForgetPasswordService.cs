using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;
using LifetimeLiveHouseWebAPI.Services.Interfaces;

namespace LifetimeLiveHouseWebAPI.Services
{
    public class ForgetPasswordService : IForgetPasswordService
    {
        private readonly LifetimeLiveHouseSysDBContext _context;
        private readonly IEmailService _emailService;
        private readonly string _frontendBaseUrl;

        public ForgetPasswordService(
            LifetimeLiveHouseSysDBContext context,
            IEmailService emailService,
            string frontendBaseUrl)
        {
            _context = context;
            _emailService = emailService;
            _frontendBaseUrl = frontendBaseUrl;
        }

        public async Task ForgotPasswordAsync(string email)
        {
            // 不做 ModelState 驗證，這裡只做業務邏輯
            var user = await _context.MemberAccount
                .SingleOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                // email 不存在也不做任何錯誤回應，為了防止帳號枚舉
                return;
            }

            string token = GenerateSecureToken();
            string hash = HashToken(token);

            var prt = new PasswordResetToken
            {
                UserId = user.Id,
                TokenHash = hash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                Used = false
            };
            _context.PasswordResetTokens.Add(prt);
            await _context.SaveChangesAsync();

            string resetLink = $"{_frontendBaseUrl}/reset-password?token={Uri.EscapeDataString(token)}";
            await _emailService.SendPasswordResetAsync(user.Email, resetLink);
        }

        private string GenerateSecureToken(int length = 32)
        {
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private string HashToken(string token)
        {
            // 這裡可以用 bcrypt 或其他安全雜湊
            // 簡單示意：
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(token);
            var hashBytes = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
