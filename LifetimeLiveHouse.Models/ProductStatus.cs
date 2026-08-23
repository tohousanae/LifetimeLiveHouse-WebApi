using System;
using System.Collections.Generic;

namespace LifetimeLiveHouse.Models;

public partial class ProductStatus
{
    public string StatusCode { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<Product> Product { get; set; } = new List<Product>();
}
