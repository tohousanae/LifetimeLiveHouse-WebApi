using Common.Helpers;
using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;

namespace LifetimeLiveHouseWebAPI.Modules.User.Services
{
    public class ForgetPasswordService(
            LifetimeLiveHouseSysDBContext context,
            IServiceScopeFactory scopeFactory,   // 改用 ScopeFactory，取代 NETCore.MailKit 的 IEmailService
            IConfiguration config,
            ILogger<ForgetPasswordService>? logger = null) : IForgetPasswordService
    {
        private readonly LifetimeLiveHouseSysDBContext _context = context;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ILogger<ForgetPasswordService>? _logger = logger;
        private readonly string _frontendBaseUrl = config["FrontendBaseUrl"] ?? "https://livetimelivehouse.sakuyaonline.uk";

        public async Task<ActionResult<string>> SendForgotPasswordEmailAsync(ForgotPasswordDto dto)
        {
            var user = await _context.MemberAccount.SingleOrDefaultAsync(u => u.Email == dto.Email);
            var responseMsg = "如果該信箱有註冊，我們已發送重設密碼信件，請檢查信件，若未收到郵件請檢察您的垃圾信件夾。";

            if (user != null)
            {
                var plainToken = TokenGeneratorHelper.GeneratePassword(100);
                var prt = new PasswordResetToken
                {
                    MemberID = user.MemberID,
                    TokenHash = BCrypt.Net.BCrypt.HashPassword(plainToken),
                };
                _context.PasswordResetToken.Add(prt);
                await _context.SaveChangesAsync();

                string resetLink = $"{_frontendBaseUrl}/reset-password?token={Uri.EscapeDataString(plainToken)}";
                var body = $"請在1小時內點擊以下連結以重設您的密碼：<br/><a href=\"{resetLink}\">{resetLink}</a>";

                // 與 MemberRegisterServices 相同做法：用獨立 Scope 呼叫自訂 OAuth2 EmailService
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
                        _logger?.LogError(ex, "Failed to send forgot-password email to {Email}", user.Email);
                    }
                });
            }

            return responseMsg;
        }

        public async Task<string> ValidResetPasswordTokenAsync(ValidResetPasswordTokenDto dto)
        {
            await CleanupExpiredTokensAsync(); // 建立新token前先清除使用過或過期的token

            dto.InputToken = Uri.UnescapeDataString(dto.InputToken); // 先解 URI

            // 因為 token 是隨機字串，所以需逐筆比對（BCrypt 雜湊不可逆）
            var validTokens = await _context.PasswordResetToken
                .Where(t => !t.Used && t.ExpiresAt > DateTime.Now)
                .ToListAsync();

            PasswordResetToken? prt = validTokens.FirstOrDefault(t => BCrypt.Net.BCrypt.Verify(dto.InputToken, t.TokenHash));

            //return $"{prt}";
            if (prt == null)
                throw new InvalidOperationException("驗證連結無效或已過期。");

            return $"{prt}";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto dto)
        {
            dto.InputToken = Uri.UnescapeDataString(dto.InputToken); // 先解 URI

            if (dto.NewPassword != dto.ConfirmPassword)
                throw new InvalidOperationException("密碼與確認密碼不一致。");

            // 因為 token 是隨機字串，所以需逐筆比對（BCrypt 雜湊不可逆）
            var validTokens = await _context.PasswordResetToken
                .Where(t => !t.Used && t.ExpiresAt > DateTime.Now)
                .ToListAsync();

            PasswordResetToken? prt = validTokens.FirstOrDefault(t => BCrypt.Net.BCrypt.Verify(dto.InputToken, t.TokenHash));

            //return $"{prt}";
            if (prt == null)
                throw new InvalidOperationException("驗證連結無效或已過期。");

            var user = await _context.MemberAccount.FirstOrDefaultAsync(a => a.MemberID == prt.MemberID);
            if (user == null)
                throw new InvalidOperationException("使用者不存在。");

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            prt.Used = true;
            prt.UsedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return "密碼已重設成功。";
        }
        // 刪除過期或使用過的token
        public async Task CleanupExpiredTokensAsync()
        {
            var expiredTokens = await _context.PasswordResetToken
                .Where(t => t.ExpiresAt < DateTime.Now || t.Used)
                .ToListAsync();

            if (expiredTokens.Any())
            {
                _context.PasswordResetToken.RemoveRange(expiredTokens);
                await _context.SaveChangesAsync();
            }
        }
    }
}
