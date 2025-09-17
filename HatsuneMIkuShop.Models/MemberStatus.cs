using LifetimeLiveHouse.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class MemberStatus
{
    [Key]
    [StringLength(1)]
    [Column(TypeName = "nchar")]
    public string StatusCode { get; set; };

    [StringLength(10)]
    public string Status { get; set; };
}