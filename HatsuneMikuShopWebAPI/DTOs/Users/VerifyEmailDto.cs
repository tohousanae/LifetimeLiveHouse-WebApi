using System.ComponentModel.DataAnnotations;

namespace LifetimeLiveHouseWebAPI.DTOs.Users
{
    public class VerifyEmailDto
    {
        [Required(ErrorMessage = "Token不能為空")]
        public string Token { get; set; } = null!;
    }
}