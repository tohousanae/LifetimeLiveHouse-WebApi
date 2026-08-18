using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouseWebAPI.Modules.User.Interfaces
{
    public interface IMemberVerificationService
    {
        Task<ActionResult<object>> VerifyEmailAsync(string token); // 回傳 Object 以支援前端 JSON 解析
        Task<ActionResult<string>> SendVerificationSMSAsync(string phoneNumber);
        Task<ActionResult<string>> VerifyPhoneAsync(long memberId, string code);
    }
}