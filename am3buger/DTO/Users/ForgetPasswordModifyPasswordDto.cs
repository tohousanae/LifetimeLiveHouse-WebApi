using Microsoft.Build.Framework;

namespace am3burger.DTO.Users
{
    public class ForgetPasswordModifyPasswordDto
    {
        [Required]
        public string? Password { get; set; }
    }
}
