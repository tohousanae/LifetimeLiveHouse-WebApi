using Common.Helpers;
using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace LifetimeLiveHouseWebAPI.Modules.User.Services
{
    public class MemberRegisterService(
        LifetimeLiveHouseSysDBContext context,
        IConfiguration config,
        IServiceScopeFactory scopeFactory,
        ILogger<MemberRegisterService>? logger = null) : IMemberRegisterService
    {
        private readonly LifetimeLiveHouseSysDBContext _context = context;
        private readonly string _frontendBaseUrl = config["FrontendBaseUrl"] ?? "https://example.com";
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ILogger<MemberRegisterService>? _logger = logger;

        public async Task<ActionResult<string>> RegisterAsync(MemberRegisterDTO dto, string? redirectUrl = null)
        {
            try
            {
                if (await _context.MemberAccount.AsNoTracking().AnyAsync(a => a.Email == dto.Email))
                    return new BadRequestObjectResult("信箱已被註冊");

                var passwordHash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(dto.Password, 10));
                var plainTokenString = TokenGeneratorHelper.GeneratePassword(100);
                var tokenHash = HashStringSHA256(plainTokenString);

                var account = new MemberAccount
                {
                    Email = dto.Email,
                    Password = passwordHash,
                    Member = new Member
                    {
                        Name = dto.Name,
                        Birthday = dto.Birthday,
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

                _context.MemberAccount.Add(account);
                await _context.SaveChangesAsync();

                var combinedToken = $"{account.Member.MemberID}:{plainTokenString}";

                if (!string.IsNullOrWhiteSpace(redirectUrl))
                {
                    var encodedRedirectUrl = Convert.ToBase64String(Encoding.UTF8.GetBytes(redirectUrl));
                    combinedToken = $"{combinedToken}:{encodedRedirectUrl}";
                }

                var emailVerifyLink = $"{_frontendBaseUrl}/verify-email?token={Uri.EscapeDataString(combinedToken)}";
                var body = $@"
                <p>您好 {dto.Name}：</p>
                <p>請點擊以下連結完成信箱驗證：</p>
                <p><a href='{emailVerifyLink}'>{emailVerifyLink}</a></p>
                <p>此連結將在 24 小時後失效。</p>";

                _ = Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
                        await emailService.SendEmailAsync(dto.Email, "會員註冊 – 信箱驗證", body);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "發送註冊信件失敗給 {Email}", dto.Email);
                    }
                });

                return dto.Name;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "RegisterAsync 發生錯誤");
                return new BadRequestObjectResult("伺服器發生錯誤");
            }
        }

        private static string HashStringSHA256(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}