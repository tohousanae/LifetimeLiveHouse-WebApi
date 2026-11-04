using LifetimeLiveHouseWebAPI.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouseWebAPI.Modules.User.Interfaces
{
    public interface IForgetPasswordService
    {
        Task<string> SendForgotPasswordEmailAsync(ForgotPasswordDto dto);

        Task<string> ResetPasswordAsync(ResetPasswordDto dto);

        Task<string> ValidResetPasswordTokenAsync(ValidResetPasswordTokenDto dto);

        Task CleanupExpiredTokensAsync();
    }
}