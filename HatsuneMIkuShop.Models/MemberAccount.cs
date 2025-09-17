using LifetimeLiveHouse.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class MemberAccount
{
    [Key]
    [StringLength(30)]
    public string Email { get; set; };

    [StringLength(200)]
    public string Password { get; set; };

    [ForeignKey("Member")]
    public long MemberID { get; set; };
}
