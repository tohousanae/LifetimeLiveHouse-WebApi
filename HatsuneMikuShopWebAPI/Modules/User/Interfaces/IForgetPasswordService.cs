using LifetimeLiveHouseWebAPI.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LifetimeLiveHouseWebAPI.Modules.User.Interfaces
{
    public interface IForgetPasswordService
    {
        Task<ActionResult<string>> SendForgotPasswordEmailAsync(ForgotPasswordDto dto);

        Task<string> ResetPasswordAsync(ResetPasswordDto dto);

        Task<string> ValidResetPasswordTokenAsync(ValidResetPasswordTokenDto dto);

        Task CleanupExpiredTokensAsync();
    }
}