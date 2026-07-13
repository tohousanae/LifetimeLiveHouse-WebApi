using System;
using System.Collections.Generic;

namespace LifetimeLiveHouseWebAPI.Models;

public partial class Product
{
    public long ProductID { get; set; }

    public string StatusCode { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public long ProductNum { get; set; }

    public decimal Pricing { get; set; }

    public decimal RetailPrice { get; set; }

    public string? Description { get; set; }

    public string Photo { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string CateID { get; set; } = null!;

    public virtual ICollection<Cart> Cart { get; set; } = new List<Cart>();

    public virtual Category Cate { get; set; } = null!;

    public virtual ICollection<Coupon> Coupon { get; set; } = new List<Coupon>();

    public virtual ProductStatus StatusCodeNavigation { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
}
