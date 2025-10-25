using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LifetimeLiveHouse.Models
{
    public class MemberVerificationStatus
    {
        [Key]
        [ForeignKey("Member")]
        public long MemberID { get; set; } 

        public bool IsPhoneVerified { get; set; } = false;

        public bool IsEmailVerified { get; set; } = false;

        IsEmailVerified
            EmailVerificationToken 
            EmailVerificationTokenExpiry = DateTime.Now.AddHours(24),
            IsPhoneVerified = false
        public virtual Member Member { get; set; } = null!;

    }
}
