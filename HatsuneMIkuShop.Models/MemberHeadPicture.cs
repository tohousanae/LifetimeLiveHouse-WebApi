using LifetimeLiveHouse.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class MemberHeadPicture
{
    [Key]
    [StringLength(50)]
    public string Picture { get; set; } = null!; // GUID+.jpg/png/webp/…

    [ForeignKey("Member")]
    public long MemberID { get; set; }

    public virtual Member? Member { get; set; } = null!;
}
