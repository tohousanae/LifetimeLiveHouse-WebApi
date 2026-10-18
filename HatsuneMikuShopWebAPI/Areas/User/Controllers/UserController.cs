using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LifetimeLiveHouseWebAPI.Areas.User.Controllers
{
    [Area("User")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 🔒 加上此標籤，確保只有已登入的 Member 才能存取
    public class UserController(IMemberProfileService profileService, IMemberVerificationService verificationService) : ControllerBase
    {
        private readonly IMemberProfileService _profileService = profileService;
        private readonly IMemberVerificationService _verificationService = verificationService;

        // 內部共用方法：從使用者的 Cookie/Token 解析出 MemberID
        private long GetCurrentMemberId()
        {
            var sidClaim = User.FindFirst(ClaimTypes.Sid)?.Value;
            return long.TryParse(sidClaim, out var id) ? id : 0;
        }

        // 📖 取得個人檔案 (包含儲值金、點數等)
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var memberId = GetCurrentMemberId();
            if (memberId == 0) return Unauthorized("無法辨識使用者身分");

            var result = await _profileService.GetMemberProfileAsync(memberId);
            return result.Result is NotFoundObjectResult notFound ? notFound : Ok(((OkObjectResult)result.Result!).Value);
        }

        // ✍️ 更新個人檔案 (根據規格，已移除性別欄位)
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] MemberUpdateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var memberId = GetCurrentMemberId();
            if (memberId == 0) return Unauthorized("無法辨識使用者身分");

            var result = await _profileService.UpdateMemberProfileAsync(memberId, dto);
            return result.Result is BadRequestObjectResult badReq ? badReq : Ok(new { Message = ((OkObjectResult)result.Result!).Value });
        }
        [HttpPost("send-email-verification")]
        public async Task<IActionResult> SendEmailVerification()
        {
            var memberId = long.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid)!.Value);
            try
            {
                var msg = await _verificationService.ResendEmailVerificationAsync(memberId);
                return Ok(new { message = msg });
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("send-phone-verification")]
        public async Task<IActionResult> SendPhoneVerification()
        {
            var memberId = long.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid)!.Value);
            try
            {
                var msg = await _verificationService.SendPhoneVerificationForCurrentMemberAsync(memberId);
                return Ok(new { message = msg });
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("verify-phone-code")]
        public async Task<IActionResult> VerifyPhoneCode([FromBody] VerifyPhoneDTO dto)
        {
            // 強制從登入憑證抓 ID，不理會前端傳來的 MemberId，徹底防範越權漏洞 (IDOR)
            var memberId = long.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid)!.Value);
            return Ok(new { message = await _verificationService.VerifyPhoneAsync(memberId, dto.Code) });
        }
    }
}