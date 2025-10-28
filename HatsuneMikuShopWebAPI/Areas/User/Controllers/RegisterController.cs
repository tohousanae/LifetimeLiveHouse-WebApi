using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Modules.Member.Interfaces;
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
    }

}
