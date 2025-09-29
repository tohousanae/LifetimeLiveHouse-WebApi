using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class BandRole
{
    [Key]
    [StringLength(1)]
    [Column(TypeName = "nchar")]
    [Required]
    public string BandRoleID { get; set; } = null!;

    [StringLength(20)]
    [Required]
    public string Role { get; set; } = null!;
}