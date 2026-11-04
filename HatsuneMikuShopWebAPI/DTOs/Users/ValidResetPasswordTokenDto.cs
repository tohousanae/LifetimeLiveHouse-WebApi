using System.ComponentModel.DataAnnotations;

namespace LifetimeLiveHouseWebAPI.DTOs.Users
{
    public class ValidResetPasswordTokenDto
    {
        [Required(ErrorMessage = "Token不能為空")]
        public string InputToken { get; set; } = null!;
    }
}