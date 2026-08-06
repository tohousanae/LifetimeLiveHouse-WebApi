namespace LifetimeLiveHouseWebAPI.Models;

public partial class PayType
{
    public string PayCode { get; set; } = null!;

    public string Type { get; set; } = null!;

    public decimal ShippingFee { get; set; }

    public virtual ICollection<Order> Order { get; set; } = new List<Order>();
}
