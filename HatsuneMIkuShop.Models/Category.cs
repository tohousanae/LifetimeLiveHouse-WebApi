using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Category
{
    [Key]
    [StringLength(5)]
    [Column(TypeName = "nchar")]
    [Required]
    public string CateID { get; set; } = null!;

    [StringLength(20)]
    [Required]
    public string CateName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}