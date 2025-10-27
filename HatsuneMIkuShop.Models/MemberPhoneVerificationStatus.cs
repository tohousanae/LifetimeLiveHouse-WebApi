using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
