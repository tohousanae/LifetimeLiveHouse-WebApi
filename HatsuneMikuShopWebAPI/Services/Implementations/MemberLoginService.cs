using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace LifetimeLiveHouseWebAPI.Services.Implementations
{
    public class MemberLoginService : IMemberLoginService
    {
        private readonly LifetimeLiveHouseSysDBContext _context;

        public MemberLoginService(LifetimeLiveHouseSysDBContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<string>> LoginAsync(LoginDTO loginDto, HttpContext httpContext)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
                return new UnauthorizedObjectResult("請輸入帳號和密碼");

            var user = await _context.MemberAccount
                .Include(u => u.Member)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                return new UnauthorizedObjectResult("帳號或密碼錯誤，請重新輸入");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Actor, user.Email),
                new Claim(ClaimTypes.Role, "Member"),
                new Claim(ClaimTypes.Sid, user.MemberID.ToString()),
                new Claim(ClaimTypes.Name, user.Member.Name)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "MemberLogin");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await httpContext.SignInAsync("MemberLogin", claimsPrincipal);

            return new OkObjectResult("登入成功");
        }

        public async Task<ActionResult<string>> LogoutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync("MemberLogin");
            return new OkObjectResult("登出成功");
        }
    }
}
