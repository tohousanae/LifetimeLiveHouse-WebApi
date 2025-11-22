using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class MemberStatus
{
    [Key]
    [StringLength(1)]
    [Column(TypeName = "nchar")]
    public string StatusCode { get; set; } = null!;

    [StringLength(10)]
    public string Status { get; set; } = null!;

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}