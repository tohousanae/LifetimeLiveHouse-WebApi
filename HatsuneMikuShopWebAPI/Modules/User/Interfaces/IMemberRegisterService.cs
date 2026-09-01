using LifetimeLiveHouseWebAPI.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouseWebAPI.Modules.User.Interfaces
{
    public interface IMemberRegisterService
    {
        Task<string> RegisterAsync(MemberRegisterDTO dto, string? redirectUrl = null);
    }
}
