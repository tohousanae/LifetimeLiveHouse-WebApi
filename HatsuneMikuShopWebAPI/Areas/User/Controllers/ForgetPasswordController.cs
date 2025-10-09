using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;

namespace LifetimeLiveHouseWebAPI.Areas.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgetPasswordController : ControllerBase
    {
        private readonly LifetimeLiveHouseSysDBContext _context;

        public ForgetPasswordController(LifetimeLiveHouseSysDBContext context)
        {
            _context = context;
        }

        // 忘記密碼 API
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            // 驗證 email 格式略過...

            var user = await _context.MemberAccount.SingleOrDefaultAsync(u => u.Email == dto.Email);
            // 不要在此早早回傳錯誤（避免告訴對方該 email 是否存在）
            // 統一回應
            var responseMsg = "如果該信箱有註冊，我們已發送重設密碼信件，請檢查信件。";

            if (user != null)
            {
                // 產生隨機 token
                string token = GenerateSecureToken();  // 例如 32 bytes, Base64 或 hex
                string hash = HashToken(token);       // SHA-256 雜湊

                var prt = new PasswordResetToken
                {
                    UserId = user.Id,
                    TokenHash = hash,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddHours(1),
                    Used = false
                };
                _db.PasswordResetTokens.Add(prt);
                await _db.SaveChangesAsync();

                // 構造重設連結（前端頁面 URL + token 作為 query param）
                string resetLink = $"{_frontendBaseUrl}/reset-password?token={Uri.EscapeDataString(token)}";
                // 寄信給 user.Email，內含 resetLink
                await _emailService.SendPasswordResetAsync(user.Email, resetLink);
            }

            return Ok(new { message = responseMsg });
        }

        // 重設密碼 API
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string token = dto.Token;
            string newPwd = dto.NewPassword;
            string confirmPwd = dto.ConfirmPassword;
            if (newPwd != confirmPwd)
            {
                return BadRequest(new { message = "密碼與確認密碼不一致。" });
            }

            string tokenHash = HashToken(token);
            var prt = await _db.PasswordResetTokens
                .Where(t => t.TokenHash == tokenHash && !t.Used && t.ExpiresAt > DateTime.UtcNow)
                .FirstOrDefaultAsync();

            if (prt == null)
            {
                return BadRequest(new { message = "重設密碼 token 無效或已過期。" });
            }

            var user = await _db.Users.FindAsync(prt.UserId);
            if (user == null)
            {
                return BadRequest(new { message = "使用者不存在。" });
            }

            // 更新密碼 — 你要用安全的雜湊方式
            user.PasswordHash = HashPassword(newPwd);
            // 標記 token 為已用
            prt.Used = true;
            prt.UsedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(new { message = "密碼已重設成功。" });
        }
    }
}
