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

            try
            {
                // 💡 1. 這裡的 result 現在會直接接收到字串 (Name)，不再是 ActionResult
                var userName = await _registerService.RegisterAsync(dto, redirectUrl);

                // 💡 2. 直接回傳 Ok 與 userName
                return Ok(new { Message = "註冊成功", Name = userName });
            }
            catch (InvalidOperationException ex)
            {
                // 💡 3. 捕捉 Service 層拋出的業務邏輯錯誤 (例如 "信箱已被註冊")
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                // 💡 4. 捕捉未預期的嚴重錯誤
                return StatusCode(500, "註冊過程發生錯誤，請稍後再試。");
            }
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