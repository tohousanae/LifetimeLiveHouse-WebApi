using System;
using System.Collections.Generic;

namespace LifetimeLiveHouseWebAPI.Models;

public partial class OrderStatus
{
    public string StatusCode { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<Order> Order { get; set; } = new List<Order>();
}
