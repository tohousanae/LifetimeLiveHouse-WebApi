using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifetimeLiveHouse.Models
{
    public class MemberPhoneVerificationStatus
    {
        [Key]
        [ForeignKey("Member")]
        public long MemberID { get; set; }

        public bool IsPhoneVerified { get; set; } = false;

        public virtual Member Member { get; set; } = null!;

    }
}
