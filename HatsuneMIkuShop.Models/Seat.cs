using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Seat
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long SeatID { get; set; }   // 主鍵 P.K

    [ForeignKey("Member")]
    public long MemberID { get; set; }

    [ForeignKey("Store")]
    public long StoreID { get; set; }
    // 導覽屬性
    //public virtual MemberStatus? MemberStatus { get; set; }
    //public virtual MemberPicture? MemberPicture { get; set; }
    //public virtual ICollection<ReBook> ReBooks { get; set; } = new List<ReBook>();
}
