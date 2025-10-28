using System.Threading.Tasks;
using LifetimeLiveHouseWebAPI.DTOs.Users;

namespace LifetimeLiveHouseWebAPI.Modules.Member.Interfaces
{
    public interface IForgetPasswordService
    {
        Task<string> SendForgotPasswordEmailAsync(ForgotPasswordDto dto);

        Task<string> ResetPasswordAsync(ResetPasswordDto dto);

        Task<string> ValidResetPasswordTokenAsync(ValidResetPasswordTokenDto dto);

        Task CleanupExpiredTokensAsync();
    }
}