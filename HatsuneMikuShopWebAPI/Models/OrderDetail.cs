using System;
using System.Collections.Generic;

namespace LifetimeLiveHouseWebAPI.Models;

public partial class OrderDetail
{
    public long OrderID { get; set; }

    public long ProductID { get; set; }

    public decimal Price { get; set; }

    public int Count { get; set; }

    public string? ShippingAddress { get; set; }

    public string ShippingMethodCode { get; set; } = null!;

    public virtual ShippingMethod ShippingMethodCodeNavigation { get; set; } = null!;

    public virtual ICollection<Order> OrdersOrder { get; set; } = new List<Order>();

    public virtual ICollection<Product> ProductsProduct { get; set; } = new List<Product>();
}
