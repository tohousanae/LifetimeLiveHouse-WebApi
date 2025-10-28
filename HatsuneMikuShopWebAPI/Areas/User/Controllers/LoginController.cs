using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Modules.Member.Interfaces;
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
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return await _loginService.LoginAsync(memberAccount, HttpContext);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<string>> Logout()
        {

            return await _loginService.LogoutAsync(HttpContext);
        }
    }
}