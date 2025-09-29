using LifetimeLiveHouse.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class EmployeeAccount
{
    [Key]
    [StringLength(30)]
    [Required]
    public string Email { get; set; } = null!;

    [StringLength(200)]
    [Required]
    public string Password { get; set; } = null!;

    [ForeignKey("Employee")]
    public long EmployeeID { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
