using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouseWebAPI.Areas.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgetPasswordController : ControllerBase
    {
        private readonly IForgetPasswordService _service;

        public ForgetPasswordController(IForgetPasswordService service)
        {
            _service = service;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _service.SendForgotPasswordEmailAsync(dto);
                return Ok(new { message = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        // 使用者點忘記密碼連結驗證token
        [HttpPost("valid-token")]
        public async Task<IActionResult> ValidResetPasswordToken([FromBody] ValidResetPasswordTokenDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _service.ValidResetPasswordTokenAsync(dto);
                return Ok(new { message = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 使用者點送出忘記密碼表單驗證token
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _service.ResetPasswordAsync(dto);
                return Ok(new { message = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}