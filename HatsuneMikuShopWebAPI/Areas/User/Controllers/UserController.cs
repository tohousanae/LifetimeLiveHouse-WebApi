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
    public class UserController(IMemberProfileService profileService) : ControllerBase
    {
        private readonly IMemberProfileService _profileService = profileService;

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
    }
}