using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Services.Implementations;
using LifetimeLiveHouseWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouseWebAPI.Areas.User.Controllers
{
    [Area("User")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IMemberRegisterServices _memberRegister;

        public RegisterController(IMemberRegisterServices memberRegister)
        {
            _memberRegister = memberRegister;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> MemberRegister(MemberRegisterDTO dto)
        {
            return await _memberRegister.MemberRegisterAsync(dto);
        }
    }

}
