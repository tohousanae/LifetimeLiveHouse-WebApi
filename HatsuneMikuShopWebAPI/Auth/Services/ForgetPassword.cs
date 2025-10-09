using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.ModelBinding;

namespace LifetimeLiveHouseWebAPI.Services
{
    public class ForgetPassword
    {
        private readonly LifetimeLiveHouseSysDBContext _context;

        public ForgetPassword(LifetimeLiveHouseSysDBContext context)
        {
            _context = context;
        }
        //8.2.1 在Service資料夾中建立CategoryService，並將CategoriesController裡的兩個Get Action相關的商業邏輯移至此撰寫
        //      (包括ItemProduct()方法一併移入CategoryService)

        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            // 驗證 email 格式略過...

            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
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
    }
}
