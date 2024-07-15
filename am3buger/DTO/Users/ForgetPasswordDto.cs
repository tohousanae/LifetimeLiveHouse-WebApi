using Microsoft.Build.Framework;

namespace am3burger.DTO.Users
{
    public class ForgetPasswordDto
    {
        [Required]
        public string? Email { get; set; }
    }
}
