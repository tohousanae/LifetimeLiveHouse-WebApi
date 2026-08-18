using LifetimeLiveHouseWebAPI.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouseWebAPI.Modules.User.Interfaces
{
    public interface IMemberProfileService
    {
        Task<ActionResult<object>> GetMemberProfileAsync(long memberId);
        Task<ActionResult<string>> UpdateMemberProfileAsync(long memberId, MemberUpdateDTO dto);
    }
}