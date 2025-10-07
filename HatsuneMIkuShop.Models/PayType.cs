
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class PayType
{
    [Key]
    [StringLength(2)]
    [Column(TypeName = "nchar")]
    public string PayCode { get; set; } = null!;

    [StringLength(10)]
    public string Type { get; set; } = null!;

    [Column(TypeName = "money")]
    [Range(0, double.MaxValue)]
    public decimal ShippingFee { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}