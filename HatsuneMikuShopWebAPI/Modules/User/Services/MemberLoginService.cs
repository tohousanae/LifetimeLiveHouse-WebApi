//using LifetimeLiveHouse.Access.Data;
//using LifetimeLiveHouseWebAPI.DTOs.Users;
//using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;

//namespace LifetimeLiveHouseWebAPI.Modules.User.Services
//{
//    public class MemberLoginService(LifetimeLiveHouseSysDBContext context) : IMemberLoginService
//    {
//        private readonly LifetimeLiveHouseSysDBContext _context = context;

//        public async Task<ActionResult<string>> LoginAsync(LoginDTO loginDto, HttpContext httpContext)
//        {
//            if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
//                return new UnauthorizedObjectResult("請輸入帳號和密碼");

//            var user = await _context.MemberAccount
//                .AsNoTracking()
//                .Where(u => u.Email == loginDto.Email)
//                .Select(u => new
//                {
//                    u.MemberID,
//                    u.Email,
//                    u.Password,
//                    u.Member.StatusCode,
//                    u.Member.Name,
//                    IsEmailVerified = u.Member.MemberEmailVerificationStatus != null && u.Member.MemberEmailVerificationStatus.IsEmailVerified,
//                    IsPhoneVerified = u.Member.MemberPhoneVerificationStatus != null && u.Member.MemberPhoneVerificationStatus.IsPhoneVerified
//                })
//                .FirstOrDefaultAsync();

//            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
//                return new UnauthorizedObjectResult("帳號或密碼錯誤，請重新輸入");

//            // 💡 停權檢查：檢查關聯表 Member 的 StatusCode 欄位 (0為正常，1為停權)
//            if (user.StatusCode == "1")
//                return new UnauthorizedObjectResult("該帳號已遭停權，請聯絡客服人員");

//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Actor, user.Email),
//                new Claim(ClaimTypes.Role, "Member"),
//                new Claim(ClaimTypes.Sid, user.MemberID.ToString()),
//                new Claim(ClaimTypes.Name, user.Name)
//            };

//            var claimsIdentity = new ClaimsIdentity(claims, "MemberLogin");
//            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

//            await httpContext.SignInAsync("MemberLogin", claimsPrincipal);

//            return new OkObjectResult("登入成功");
//        }

//        public async Task<ActionResult<string>> LogoutAsync(HttpContext httpContext)
//        {
//            await httpContext.SignOutAsync("MemberLogin");
//            return new OkObjectResult("登出成功");
//        }
//    }
//}