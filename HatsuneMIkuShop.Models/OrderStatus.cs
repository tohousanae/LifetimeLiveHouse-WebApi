
using System.ComponentModel.DataAnnotations;
public partial class OrderStatus
{
    [Key]
    [StringLength(1)]
    public string StatusCode { get; set; } = null!;

    [StringLength(10)]
    public string Status { get; set; } = null!;

    // 導覽屬性
    //public virtual MemberStatus? MemberStatus { get; set; }
    //public virtual MemberPicture? MemberPicture { get; set; }
    //public virtual ICollection<ReBook> ReBooks { get; set; } = new List<ReBook>();
}