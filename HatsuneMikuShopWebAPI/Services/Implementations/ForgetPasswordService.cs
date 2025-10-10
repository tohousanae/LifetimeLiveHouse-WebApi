using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Helpers;
using LifetimeLiveHouseWebAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;
using System.Security.Cryptography;
using System.Text;

namespace LifetimeLiveHouseWebAPI.Services.Implementations
{
    public class ForgetPasswordService : IForgetPasswordService
    {
        private readonly LifetimeLiveHouseSysDBContext _db;
        private readonly IEmailService _emailService; // 假設你已有這個服務
        private readonly string _frontendBaseUrl;

        public ForgetPasswordService(
            LifetimeLiveHouseSysDBContext db,
            IEmailService emailService,
            IConfiguration config)
        {
            _db = db;
            _emailService = emailService;
            _frontendBaseUrl = config["FrontendBaseUrl"] ?? "https://example.com"; // 可放 appsettings.json
        }

        public async Task<string> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _db.MemberAccount.SingleOrDefaultAsync(u => u.Email == dto.Email);
            var responseMsg = "如果該信箱有註冊，我們已發送重設密碼信件，請檢查信件。";

            if (user != null)
            {
                // 產生 token
                string token = PasswordTokenHelper.GeneratePassword(100);
                string hash = BCrypt.Net.BCrypt.HashPassword(token);

                var prt = new PasswordResetToken
                {
                    MemberID = user.MemberID,
                    TokenHash = hash,
                    CreatedAt = DateTime.Now,
                    ExpiresAt = DateTime.Now.AddHours(1),
                    Used = false
                };
                _db.PasswordResetToken.Add(prt);
                await _db.SaveChangesAsync();

                // 建立重設連結
                string resetLink = $"{_frontendBaseUrl}/reset-password?token={Uri.EscapeDataString(token)}";

                await _emailService.SendAsync(
                    user.Email,
                    "重設密碼通知",
                    $"請點擊以下連結以重設您的密碼：<br/><a href=\"{resetLink}\">{resetLink}</a>",
                    isHtml: true
                );
            }

            return responseMsg;
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
                throw new InvalidOperationException("密碼與確認密碼不一致。");

            // 因為 token 是隨機字串，所以需逐筆比對（BCrypt 雜湊不可逆）
            var validTokens = await _db.PasswordResetToken
                .Where(t => !t.Used && t.ExpiresAt > DateTime.Now)
                .ToListAsync();

            PasswordResetToken? prt = null;
            foreach (var tokenRecord in validTokens)
            {
                if (BCrypt.Net.BCrypt.Verify(dto.inputToken, tokenRecord.TokenHash))
                {
                    prt = tokenRecord;
                    break;
                }
            }

            if (prt == null)
                throw new InvalidOperationException("重設密碼 token 無效或已過期。");

            var user = await _db.MemberAccount.FindAsync(prt.MemberID);
            if (user == null)
                throw new InvalidOperationException("使用者不存在。");

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            prt.Used = true;
            prt.UsedAt = DateTime.Now;

            await _db.SaveChangesAsync();
            return "密碼已重設成功。";
        }
    }
}
