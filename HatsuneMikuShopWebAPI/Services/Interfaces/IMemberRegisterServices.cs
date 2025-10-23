using LifetimeLiveHouseWebAPI.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouseWebAPI.Services.Interfaces
{
    public interface IMemberRegisterServices
    {
        Task<ActionResult<string>> MemberRegisterAsync(MemberRegisterDTO dto);
        Task<ActionResult<string>> VerifyEmailAsync(long accountId, string token);
        Task<ActionResult<string>> VerifyPhoneAsync(long accountId, string code):
    }
}
