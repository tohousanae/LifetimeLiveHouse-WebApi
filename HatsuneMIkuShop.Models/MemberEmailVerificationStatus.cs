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

        public string? EmailVerificationTokenHash { get; set; }

        public DateTime? EmailVerificationTokenExpiry { get; set; }

        public virtual Member Member { get; set; } = null!;

    }
}
