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

        // 導覽屬性
        //public virtual MemberStatus? MemberStatus { get; set; }
        //public virtual MemberPicture? MemberPicture { get; set; }
        //public virtual ICollection<ReBook> ReBooks { get; set; } = new List<ReBook>();

    }
}
