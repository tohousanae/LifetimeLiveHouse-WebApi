using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Helpers;
using LifetimeLiveHouseWebAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;

namespace LifetimeLiveHouseWebAPI.Services.Implementations
{
    public class ForgetPasswordService(
        LifetimeLiveHouseSysDBContext db,
        IEmailService emailService,
        IConfiguration config) : IForgetPasswordService
    {
        private readonly LifetimeLiveHouseSysDBContext _db = db;
        private readonly IEmailService _emailService = emailService; // 假設你已有這個服務
        private readonly string _frontendBaseUrl = config["FrontendBaseUrl"] ?? "https://example.com";

        public async Task<string> SendForgotPasswordEmailAsync(ForgotPasswordDto dto)
        {
            var user = await _db.MemberAccount.SingleOrDefaultAsync(u => u.Email == dto.Email);
            var responseMsg = "如果該信箱有註冊，我們已發送重設密碼信件，請檢查信件，若未收到郵件請檢察您的垃圾信件夾。";

            if (user != null)
            {
                // 產生 token
                string token = PasswordTokenHelper.GeneratePassword(100);
                string hash = BCrypt.Net.BCrypt.HashPassword(token);

                var prt = new PasswordResetToken
                {
                    MemberID = user.MemberID,
                    TokenHash = hash,
                    //CreatedAt = DateTime.Now, /已在模型設定
                    //ExpiresAt = DateTime.Now.AddHours(1),
                    //Used = false
                };
                _db.PasswordResetToken.Add(prt);
                await _db.SaveChangesAsync();

                // 建立重設連結
                string resetLink = $"{_frontendBaseUrl}/reset-password?token={Uri.EscapeDataString(token)}";

                await _emailService.SendAsync(
                    user.Email,
                    "重設密碼通知",
                    $"請在1小時內點擊以下連結以重設您的密碼：<br/><a href=\"{resetLink}\">{resetLink}</a>",
                    isHtml: true
                );
            }

            return responseMsg;
        }

        public async Task<string> ValidResetPasswordTokenAsync(ValidResetPasswordTokenDto dto)
        {
            await CleanupExpiredTokensAsync(); // 建立新token前先清除使用過或過期的token

            dto.InputToken = Uri.UnescapeDataString(dto.InputToken); // 先解 URI

            // 因為 token 是隨機字串，所以需逐筆比對（BCrypt 雜湊不可逆）
            var validTokens = await _db.PasswordResetToken
                .Where(t => !t.Used && t.ExpiresAt > DateTime.Now)
                .ToListAsync();

            PasswordResetToken? prt = validTokens.FirstOrDefault(t => BCrypt.Net.BCrypt.Verify(dto.InputToken, t.TokenHash));

            //return $"{prt}";
            if (prt == null)
                throw new InvalidOperationException("重設密碼 token 無效或已過期。");

            return $"{prt}";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto dto)
        {
            dto.InputToken = Uri.UnescapeDataString(dto.InputToken); // 先解 URI

            if (dto.NewPassword != dto.ConfirmPassword)
                throw new InvalidOperationException("密碼與確認密碼不一致。");

            // 因為 token 是隨機字串，所以需逐筆比對（BCrypt 雜湊不可逆）
            var validTokens = await _db.PasswordResetToken
                .Where(t => !t.Used && t.ExpiresAt > DateTime.Now)
                .ToListAsync();

            PasswordResetToken? prt = validTokens.FirstOrDefault(t => BCrypt.Net.BCrypt.Verify(dto.InputToken, t.TokenHash));

            //return $"{prt}";
            if (prt == null)
                throw new InvalidOperationException("重設密碼 token 無效或已過期。");

            var user = await _db.MemberAccount.FirstOrDefaultAsync(a => a.MemberID == prt.MemberID);
            if (user == null)
                throw new InvalidOperationException("使用者不存在。");

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            prt.Used = true;
            prt.UsedAt = DateTime.Now;

            await _db.SaveChangesAsync();
            return "密碼已重設成功。";
        }
        // 刪除過期或使用過的token
        public async Task CleanupExpiredTokensAsync()
        {
            var expiredTokens = await _db.PasswordResetToken
                .Where(t => t.ExpiresAt < DateTime.Now || t.Used)
                .ToListAsync();

            if (expiredTokens.Any())
            {
                _db.PasswordResetToken.RemoveRange(expiredTokens);
                await _db.SaveChangesAsync();
            }
        }
    }
}
