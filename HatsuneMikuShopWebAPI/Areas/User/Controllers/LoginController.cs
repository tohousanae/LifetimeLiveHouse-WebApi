using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LifetimeLiveHouseWebAPI.Areas.User.Controllers
{
    [Area("User")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(LifetimeLiveHouseSysDBContext context) : ControllerBase
    {
        private readonly LifetimeLiveHouseSysDBContext _context = context;

        // 登入api(cookie based驗證)
        [HttpPost("login")]

        public async Task<ActionResult<MemberAccount>> PostUserLogin(LoginDTO memberAccount)
        {
            if (memberAccount == null || string.IsNullOrEmpty(memberAccount.Email) || string.IsNullOrEmpty(memberAccount.Password))
            {
                return Unauthorized("請輸入帳號和密碼");
            }

            var user = await _context.MemberAccount.Include(u => u.Member)   //密碼必須先經過雜湊處理，再與資料庫中的密碼進行比對
                .FirstOrDefaultAsync(u => u.Email == memberAccount.Email);

            if (!BCrypt.Net.BCrypt.Verify(memberAccount.Password, user.Password))
            {
                return Unauthorized("帳號或密碼錯誤，請重新輸入"); // 刻意將回傳訊息設定成信箱或密碼錯誤，防止攻擊者針對密碼做攻擊測試
            }
            if (user != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Actor, user.Email),
                        new Claim(ClaimTypes.Role, "Member"),
                         new Claim(ClaimTypes.Sid, Convert.ToString(user.MemberID)),
                          new Claim(ClaimTypes.Name, user.Member.Name)

                    };

                var claimsIdentity = new ClaimsIdentity(claims, "MemberLogin");

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync("MemberLogin", claimsPrincipal); //把資料寫入 Cookie 進行登入狀態管理

                return Ok("登入成功"); 
            }

            return Unauthorized("帳號或密碼錯誤，請重新輸入");
        }

        // 登出，清除cookie
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MemberLogin");// 清除登入狀態(清除Cookie的MemberLogin紀錄)
            return Ok("登出成功");
        }
    }
}