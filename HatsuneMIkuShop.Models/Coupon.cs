using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Coupon
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long cNo { get; set; }   // 主鍵 P.K

    [StringLength(200)]
    public string? cDesc { get; set; }

    [Column(TypeName = "money")]
    [Range(0, double.MaxValue)]
    public decimal Discount { get; set; } = 0;

    [ForeignKey("Product")]
    public long ProductID { get; set; }

    [ForeignKey("Member")]
    public long MemberID { get; set; }
    // 導覽屬性
    //public virtual MemberStatus? MemberStatus { get; set; }
    //public virtual MemberPicture? MemberPicture { get; set; }
    //public virtual ICollection<ReBook> ReBooks { get; set; } = new List<ReBook>();
}