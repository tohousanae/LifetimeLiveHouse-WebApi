using System.Threading.Tasks;
using LifetimeLiveHouseWebAPI.DTOs.Users;

namespace LifetimeLiveHouseWebAPI.Services.Interfaces
{
    public interface IForgetPasswordService
    {
        Task<string> ForgotPasswordAsync(ForgotPasswordDto dto);

        Task<string> ResetPasswordAsync(ResetPasswordDto dto);

        Task CleanupExpiredTokensAsync();
    }
}