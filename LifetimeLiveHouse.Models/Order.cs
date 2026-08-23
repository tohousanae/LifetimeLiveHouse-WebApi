using System;
using System.Collections.Generic;

namespace LifetimeLiveHouse.Models;

public partial class Order
{
    public long OrderID { get; set; }

    public DateTime OrderDate { get; set; }

    public string oTel { get; set; } = null!;

    public string? Note { get; set; }

    public long MemberID { get; set; }

    public long? EmployeeID { get; set; }

    public string? PayCode { get; set; }

    public string StatusCode { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual PayType? PayCodeNavigation { get; set; }

    public virtual OrderStatus StatusCodeNavigation { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
}
