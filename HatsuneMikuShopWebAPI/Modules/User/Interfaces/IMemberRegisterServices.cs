using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouseWebAPI.Modules.User.Interfaces
{
    public interface IMemberRegisterServices
    {
        Task<ActionResult<string>> RegisterAsync(MemberRegisterDTO dto);

        Task<ActionResult<string>> SendVerificationSMSAsync(string phoneNumber);
 
        Task<ActionResult<string>> VerifyEmailAsync(long memberId, string token);
        
        Task<ActionResult<string>> VerifyPhoneAsync(long memberId, string code);
    }
}
