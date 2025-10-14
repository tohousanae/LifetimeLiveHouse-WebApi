using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LifetimeLiveHouseWebAPI.Areas.User.Controllers
{
    [Area("User")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMemberLoginService _loginService;

        public LoginController(IMemberLoginService loginService)
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
    }
}