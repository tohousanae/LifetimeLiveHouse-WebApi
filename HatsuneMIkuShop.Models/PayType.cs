
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class PayType
{
    [Key]
    [StringLength(2)]
    [Column(TypeName = "nchar")]
    public string PayCode { get; set; } = null!;

    [StringLength(10)]
    public string Type { get; set; } = null!;

    // 導覽屬性
    //public virtual MemberStatus? MemberStatus { get; set; }
    //public virtual MemberPicture? MemberPicture { get; set; }
    //public virtual ICollection<ReBook> ReBooks { get; set; } = new List<ReBook>();
}