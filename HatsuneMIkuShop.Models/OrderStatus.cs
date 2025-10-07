
using System.ComponentModel.DataAnnotations;
public partial class OrderStatus
{
    [Key]
    [StringLength(1)]
    public string StatusCode { get; set; } = null!;

    [StringLength(10)]
    public string Status { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}