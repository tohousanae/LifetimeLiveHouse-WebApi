using Common.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifetimeLiveHouse.Models
{
    public class MemberEmailVerificationStatus
    {
        [Key]
        [ForeignKey("Member")]
        public long MemberID { get; set; } 

        public bool IsEmailVerified { get; set; } = false;

        [Required]
        public string? EmailVerificationTokenHash { get; set; } = string.Empty;

        public DateTime? EmailVerificationTokenExpiry { get; set; } = DateTime.Now.AddHours(24);

        public virtual Member Member { get; set; } = null!;

    }
}
