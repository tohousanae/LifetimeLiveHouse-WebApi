using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifetimeLiveHouseWebAPI.Areas.User.Controllers
{
    [Area("User")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly LifetimeLiveHouseSysDBContext _context;

        public RegisterController(LifetimeLiveHouseSysDBContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult> PostUserFullRegister(MemberRegisterDTO input)
        {
            private readonly IMemberRegisterService _registerService;

        public RegisterController(IMemberRegisterService RegisterController)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> PostUserLogin(LoginDTO memberAccount)
        {
            return await _loginService.LoginAsync(memberAccount, HttpContext);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<string>> Logout()
        {
            return await _loginService.LogoutAsync(HttpContext);
        }

            // 檢查 Email / 手機
            if (await _context.MemberAccount.AnyAsync(u => u.Email == input.Email))
                return Unauthorized("信箱已被註冊");

            if (await _context.Member.AnyAsync(u => u.CellphoneNumber == input.CellphoneNumber))
                return Unauthorized("手機號碼已被註冊");

        var user = ConvertToUser(new MemberRegisterDTO
        {
            Name = input.Name,
            Password = input.Password,
            Birthday = input.Birthday,
            CellphoneNumber = input.CellphoneNumber,
            StatusCode = "0" // 預設正常

        });

        var userAccount = new MemberAccount
        {
            Email = input.Email,
            Password = user.Password,
            Member = user
        };

        _context.Member.Add(user);
            _context.MemberAccount.Add(userAccount);

            await _context.SaveChangesAsync();

            return Ok("完整會員註冊成功");
    }

}
}
