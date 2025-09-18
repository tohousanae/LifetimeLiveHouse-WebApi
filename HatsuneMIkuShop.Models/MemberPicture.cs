using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class MemberPicture
{
    [Key]
    [StringLength(30)]
    public string Picture { get; set; } = null!;

    [StringLength(200)]
    public string Password { get; set; } = null!;

    [ForeignKey("Member")]
    public long MemberID { get; set; }
}
