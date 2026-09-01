using Common.Helpers;
using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifetimeLiveHouseWebAPI.Modules.User.Services
{
    // 💡 1. 在建構子注入 IWebHostEnvironment env 來判斷環境
    public class ForgetPasswordService(
            LifetimeLiveHouseSysDBContext context,
            IServiceScopeFactory scopeFactory,
            IConfiguration config,
            IWebHostEnvironment env,
            ILogger<ForgetPasswordService>? logger = null) : IForgetPasswordService
    {
        private readonly LifetimeLiveHouseSysDBContext _context = context;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly IWebHostEnvironment _env = env; // 💡 2. 接收環境變數
        private readonly ILogger<ForgetPasswordService>? _logger = logger;
        private readonly string _frontendBaseUrl = config["FrontendBaseUrl"] ?? "https://livetimelivehouse.sakuyaonline.uk";

        public async Task<string> SendForgotPasswordEmailAsync(ForgotPasswordDto dto)
        {
            var user = await _context.MemberAccount.SingleOrDefaultAsync(u => u.Email == dto.Email);
            var responseMsg = "如果該信箱有註冊，我們已發送重設密碼信件，請檢查信件，若未收到郵件請檢查您的垃圾信件夾。";

            if (user != null)
            {
                var plainToken = TokenGeneratorHelper.GeneratePassword(100);

                var prt = new PasswordResetToken
                {
                    MemberID = user.MemberID,
                    TokenHash = BCrypt.Net.BCrypt.HashPassword(plainToken),
                    CreatedAt = DateTime.Now,
                    ExpiresAt = DateTime.Now.AddHours(1),
                    Used = false
                };

                _context.PasswordResetToken.Add(prt);
                await _context.SaveChangesAsync();

                string resetLink = $"{_frontendBaseUrl}/reset-password?token={Uri.EscapeDataString(plainToken)}";
                var body = $"請在1小時內點擊以下連結以重設您的密碼：<br/><a href=\"{resetLink}\">{resetLink}</a>";

                // 💡 3. 根據環境決定寄信策略
                if (_env.IsDevelopment())
                {
                    // 【開發/偵錯模式】直接等待寄信完成 (不包 try-catch)。
                    // 只要 Google 驗證失敗，會立刻拋出例外，API 請求會直接得到 500 錯誤！
                    using var scope = _scopeFactory.CreateScope();
                    var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
                    await emailService.SendEmailAsync(user.Email, "重設密碼通知", body);
                }
                else
                {
                    // 【正式環境模式】丟進背景 Task.Run 執行。
                    // 不阻塞主執行緒，失敗時只記錄 Log，保護系統不會因為寄信失敗而中斷[cite: 20]。
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

            return responseMsg;
        }

        public async Task<string> ValidResetPasswordTokenAsync(ValidResetPasswordTokenDto dto)
        {
            //await CleanupExpiredTokensAsync();

            dto.InputToken = Uri.UnescapeDataString(dto.InputToken);

            var validTokens = await _context.PasswordResetToken
                .Where(t => !t.Used && t.ExpiresAt > DateTime.Now)
                .ToListAsync();

            PasswordResetToken? prt = validTokens.FirstOrDefault(t => BCrypt.Net.BCrypt.Verify(dto.InputToken, t.TokenHash));

            if (prt == null)
                throw new InvalidOperationException("驗證連結無效或已過期。");

            return "Token 有效";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto dto)
        {
            dto.InputToken = Uri.UnescapeDataString(dto.InputToken);

            if (dto.NewPassword != dto.ConfirmPassword)
                throw new InvalidOperationException("密碼與確認密碼不一致。");

            var validTokens = await _context.PasswordResetToken
                .Where(t => !t.Used && t.ExpiresAt > DateTime.Now)
                .ToListAsync();

            PasswordResetToken? prt = validTokens.FirstOrDefault(t => BCrypt.Net.BCrypt.Verify(dto.InputToken, t.TokenHash));

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

        public async Task CleanupExpiredTokensAsync()
        {
            await _context.PasswordResetToken
                .Where(t => t.ExpiresAt < DateTime.Now || t.Used)
                .ExecuteDeleteAsync();
        }
    }
}