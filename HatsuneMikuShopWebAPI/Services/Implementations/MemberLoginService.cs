using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LifetimeLiveHouseWebAPI.Services.Implementations
{
    public class MemberLoginService : IMemberLoginService
    {
        private readonly LifetimeLiveHouseSysDBContext _context;

        public MemberLoginService(LifetimeLiveHouseSysDBContext context)
        {
            _context = context;
        }

        //public async Task<ActionResult<string>> LoginAsync(LoginDTO loginDto, HttpContext httpContext)
        //{
        //    if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
        //        return new UnauthorizedObjectResult("請輸入帳號和密碼");

        //    var user = await _context.MemberAccount
        //        .Include(u => u.Member)
        //        .Include(u => u.Member.MemberVerificationStatus)
        //        .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        //    if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        //        return new UnauthorizedObjectResult("帳號或密碼錯誤，請重新輸入");

        //    if (user.Member.StatusCode == "1")
        //        return new UnauthorizedObjectResult("該帳號已停權，請檢察您的電子郵件");

        //    if (user.Member.MemberVerificationStatus.EmailVerificationStatus == false)
        //    {
        //        return new UnauthorizedObjectResult("未完成電子郵件驗證");
        //    }
        //    if (user.Member.MemberVerificationStatus.PhoneVerificationStatus == false)
        //    {
        //        return new UnauthorizedObjectResult("未完成手機號碼驗證");
        //    }

        //    //if (user.Member.)
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Actor, user.Email),
        //        new Claim(ClaimTypes.Role, "Member"),
        //        new Claim(ClaimTypes.Sid, user.MemberID.ToString()),
        //        new Claim(ClaimTypes.Name, user.Member.Name)
        //    };

        //    var claimsIdentity = new ClaimsIdentity(claims, "MemberLogin");
        //    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        //    await httpContext.SignInAsync("MemberLogin", claimsPrincipal);

        //    return new OkObjectResult("登入成功");
        //}
        public async Task<ActionResult<string>> LoginAsync(LoginDTO loginDto, HttpContext httpContext)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
                return new UnauthorizedObjectResult("請輸入帳號和密碼");

            var user = await _context.MemberAccount
                .AsNoTracking()
                .Where(u => u.Email == loginDto.Email)
                .Select(u => new
                {
                    u.MemberID,
                    u.Email,
                    u.Password,
                    u.Member.StatusCode,
                    u.Member.Name,
                    u.Member.MemberVerificationStatus.PhoneVerificationStatus,
                    u.Member.MemberVerificationStatus.EmailVerificationStatus
                })
                .FirstOrDefaultAsync();

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                return new UnauthorizedObjectResult("帳號或密碼錯誤，請重新輸入");

            if (user.StatusCode == "1")
                return new UnauthorizedObjectResult("該帳號已停權，請檢察您的電子郵件");

            if (!user.PhoneVerificationStatus)
                return new UnauthorizedObjectResult("未完成手機號碼驗證");

            if (!user.EmailVerificationStatus)
                return new UnauthorizedObjectResult("未完成電子郵件驗證");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Actor, user.Email),
                new Claim(ClaimTypes.Role, "Member"),
                new Claim(ClaimTypes.Sid, user.MemberID.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
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
