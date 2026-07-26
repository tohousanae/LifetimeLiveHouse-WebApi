namespace LifetimeLiveHouseWebAPI.Models;

public partial class ShippingMethod
{
    public string ShippingMethodCode { get; set; } = null!;

    public string Method { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
}
