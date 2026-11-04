using System.ComponentModel.DataAnnotations;

namespace LifetimeLiveHouseWebAPI.DTOs.Users
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "信箱不能為空")]
        [EmailAddress(ErrorMessage = "信箱格式錯誤")]
        [StringLength(30, ErrorMessage = "信箱長度最多 30 字元")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "密碼不能為空")]
        public string Password { get; set; } = null!;
    }
}