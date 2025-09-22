using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class BandRole
{
    [Key]
    [StringLength(1)]
    [Column(TypeName = "nchar")]
    public string BandRoleID { get; set; } = null!;

    [StringLength(20)]
    public string Role { get; set; } = null!;
}