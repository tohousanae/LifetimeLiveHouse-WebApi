using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouseWebAPI.Modules.User.Interfaces
{
    public interface IMemberRegisterServices
    {
        Task<ActionResult<string>> MemberRegisterAsync(MemberRegisterDTO dto);
        Task<ActionResult<string>?> CheckEmailOrCellphoneAlreadyRegisteredAsync(string email, string cellphoneNumber);
        Task<Member> InsertMemberAsync(MemberRegisterDTO dto);
        Task<MemberAccount> InsertMemberAccountAsync(long memberId, string email, string password);
        Task<MemberEmailVerificationStatus> InsertMemberEmailVerificationStatusAsync(long memberId);
        Task<MemberPhoneVerificationStatus> InsertMemberPhoneVerificationStatusAsync(long memberId);
        Task<ActionResult<string>> VerifyEmailAsync(long memberId, string token);
        Task<ActionResult<string>> VerifyPhoneAsync(long memberId, string code);
    }
}
