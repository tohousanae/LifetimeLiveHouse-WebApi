using LifetimeLiveHouseWebAPI.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouseWebAPI.Services.Interfaces
{
    public interface IMemberRegisterServices
    {
        Task<ActionResult<string>> MemberRegisterAsync(MemberRegisterDTO dto);

        Task<(Member member, MemberAccount account)> InsertMemberAsync(MemberRegisterDTO member);
    }
}
