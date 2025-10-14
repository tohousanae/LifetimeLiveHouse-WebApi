using LifetimeLiveHouseWebAPI.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouseWebAPI.Services.Interfaces
{
    public interface IMemberLoginService
    {
        Task<ActionResult<string>> LoginAsync(LoginDTO loginDto, HttpContext httpContext);
        Task<ActionResult<string>> LogoutAsync(HttpContext httpContext);
    }
}
