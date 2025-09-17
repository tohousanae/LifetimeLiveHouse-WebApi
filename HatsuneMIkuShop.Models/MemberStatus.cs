using LifetimeLiveHouse.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class MemberStatus
{
    [Key]
    [StringLength(1)]
    public string Email { get; set; } = null!;

    [StringLength(200)]
    public string Password { get; set; } = null!;

    [ForeignKey("Member")]
    public long MemberID { get; set; }
}