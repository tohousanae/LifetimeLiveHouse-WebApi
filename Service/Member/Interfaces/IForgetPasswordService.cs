using System.Threading.Tasks;
using LifetimeLiveHouseWebAPI.DTOs.Users;

namespace LifetimeLiveHouse.Services.Member.Interfaces
{
    public interface IForgetPasswordService
    {
        Task<string> SendForgotPasswordEmailAsync(ForgotPasswordDto dto);

        Task<string> ResetPasswordAsync(ResetPasswordDto dto);

        Task<string> ValidResetPasswordTokenAsync(ValidResetPasswordTokenDto dto);

        Task CleanupExpiredTokensAsync();
    }
}