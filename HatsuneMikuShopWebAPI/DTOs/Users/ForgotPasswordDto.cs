using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifetimeLiveHouseWebAPI.DTOs.Users
{
    public class ForgotPasswordDto
    {
        [Key]
        [StringLength(30)]
        public string Email { get; set; } = null!;

        [StringLength(200)]
        public string Password { get; set; } = null!;

        [ForeignKey("Member")]
        public long MemberID { get; set; }

        public virtual Member Member { get; set; } = null!;
    }
}
