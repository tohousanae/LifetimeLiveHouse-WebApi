using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouseWebAPI.Areas.User.Controllers
{
    [Area("User")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController(
        IMemberRegisterService registerService,
        IMemberVerificationService verificationService) : ControllerBase
    {
        private readonly IMemberRegisterService _registerService = registerService;
        private readonly IMemberVerificationService _verificationService = verificationService;

        // 📝 註冊帳號 (允許未登入客訪問)
        [AllowAnonymous]
        [HttpPost("postRegisterMember")]
        public async Task<IActionResult> Register(MemberRegisterDTO dto, [FromQuery] string? redirectUrl = null)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _registerService.RegisterAsync(dto, redirectUrl);

            // 若回傳型態為 BadRequest，直接回傳錯誤訊息
            if (result.Result is BadRequestObjectResult badReq) return badReq;

            return Ok(new { Message = "註冊成功", Name = result.Value });
        }

        // ✉️ 信箱連結驗證 (改用 POST 與 Body 接收，徹底隱藏 Token)
        [AllowAnonymous]
        [HttpPost("verify-email")]
        public async Task<ActionResult<object>> VerifyEmail([FromBody] VerifyEmailDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // 從 dto.Token 取出驗證碼傳給 Service
            return await _verificationService.VerifyEmailAsync(dto.Token);
        }

        // 📱 發送手機簡訊驗證碼
        [HttpPost("sendValidationSMS")]
        public async Task<ActionResult<string>> SendValidationSMS([FromBody] UserPhoneNumberDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return await _verificationService.SendVerificationSMSAsync(dto.CellphoneNumber);
        }

        // 📱 驗證手機簡訊碼
        [HttpPost("verify-phone")]
        public async Task<ActionResult<string>> VerifyPhone([FromBody] VerifyPhoneDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return await _verificationService.VerifyPhoneAsync(dto.MemberId, dto.Code);
        }
    }
}