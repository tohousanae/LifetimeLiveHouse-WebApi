using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LifetimeLiveHouse.Models
{
    public class MemberVerificationStatus
    {
        [Key]
        [ForeignKey("Member")]
        public long MemberID { get; set; } 

        public bool PhoneVerificationStatus { get; set; } = false;

        public bool EmailVerificationStatus { get; set; } = false;

        public virtual Member Member { get; set; } = null!;

    }
}
