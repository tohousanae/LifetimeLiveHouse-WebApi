using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifetimeLiveHouseWebAPI.DTOs.Users
{
    public class ForgotPasswordDto
    {
        public string Email { get; set; } = null!;
    }
}
