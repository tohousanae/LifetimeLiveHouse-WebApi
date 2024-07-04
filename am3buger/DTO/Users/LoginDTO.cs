using System.ComponentModel.DataAnnotations;

namespace am3burger.DTO.Users
{
    public class LoginDTO
    {
        [Required]
        [Display(Name = "信箱")]
        [StringLength(50)]
        // 信箱
        public string? Email { get; set; }

        [Required]
        [Display(Name = "密碼")]
        [StringLength(50)]
        // 密碼
        public string? Password { get; set; }
    }
}
