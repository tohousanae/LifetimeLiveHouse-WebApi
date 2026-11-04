using System;
using System.Collections.Generic;

namespace LifetimeLiveHouse.Models;

public partial class Coupon
{
    public long cNo { get; set; }

    public string? cDesc { get; set; }

    public decimal Discount { get; set; }

    public long ProductID { get; set; }

    public long MemberID { get; set; }

    public DateTime GetCouponDate { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool Used { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
