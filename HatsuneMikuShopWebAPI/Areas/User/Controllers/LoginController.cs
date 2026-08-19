using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouseWebAPI.Areas.User.Controllers
{
    [Area("User")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(IMemberLoginService loginService) : ControllerBase
    {
        private readonly IMemberLoginService _loginService = loginService;

        // 🔑 登入
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> PostUserLogin([FromBody] LoginDTO memberAccount)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return await _loginService.LoginAsync(memberAccount, HttpContext);
        }

        // 🚪 登出
        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<string>> Logout()
        {
            return await _loginService.LogoutAsync(HttpContext);
        }
    }
}