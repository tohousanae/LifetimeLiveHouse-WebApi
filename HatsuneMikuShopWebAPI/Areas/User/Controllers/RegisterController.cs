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
        public async Task<ActionResult<string>> MemberRegister(MemberRegisterDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return await _memberRegister.MemberRegisterAsync(dto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
        [HttpGet("verify-email")]
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
