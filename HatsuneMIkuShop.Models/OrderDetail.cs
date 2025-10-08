using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[PrimaryKey("OrderID", "ProductID")]
public class OrderDetail
{
    [ForeignKey("Order")]
    public long OrderID { get; set; }

    [ForeignKey("Product")]
    public long ProductID { get; set; }

    [Column(TypeName = "money")]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Count{ get; set; }

    [StringLength(50)]
    public string? ShippingAddress { get; set; }

    [StringLength(1)]
    [Column(TypeName = "nchar")]
    [ForeignKey("ShippingMethod")]
    public string ShippingMethodCode { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ShippingMethod ShippingMethod { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
