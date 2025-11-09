using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouseWebAPI.Areas.User.Controllers
{
    [Area("User")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController(IMemberRegisterServices memberRegister) : ControllerBase
    {
        private readonly IMemberRegisterServices _memberRegister = memberRegister;

        [HttpPost("register")]
        public async Task<IActionResult> Register(MemberRegisterDTO dto)
        {
            var result = await _memberRegister.RegisterAsync(dto);
            if (result.Result is BadRequestObjectResult badReq)
            {
                return badReq;
            }
            return Ok(new { Message = "註冊成功", Name = result.Value });
        }
        [HttpPost("sendValidationSMS")]
        public async Task<ActionResult<string>> SendValidationSMS(UserPhoneNumberDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return await _memberRegister.SendVerificationSMSAsync(dto.CellphoneNumber);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("verify-email")]
        public async Task<ActionResult<string>> VerifyEmail([FromQuery] long memberId, [FromQuery] string token)
        {
            return await _memberRegister.VerifyEmailAsync(memberId, token);
        }

        [HttpPost("verify-phone")]
        public async Task<ActionResult<string>> VerifyPhone([FromBody] VerifyPhoneDTO dto)
        {
            return await _memberRegister.VerifyPhoneAsync(dto.MemberId, dto.Code);
        }
    }

}
