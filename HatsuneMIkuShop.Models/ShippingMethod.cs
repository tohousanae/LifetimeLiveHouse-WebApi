using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ShippingMethod
{
    [Key]
    [StringLength(1)]
    [Column(TypeName = "nchar")]
    public string ShippingMethodCode { get; set; } = null!;
    
    [StringLength(10)]
    public string Method { get; set; } = null!;

    public virtual ICollection<ShippingMethod> ShippingMethods { get; set; } = new List<ShippingMethod>();
}