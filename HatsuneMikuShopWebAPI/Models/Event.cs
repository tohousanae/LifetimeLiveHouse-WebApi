using System;
using System.Collections.Generic;

namespace LifetimeLiveHouseWebAPI.Models;

public partial class Event
{
    public long EventID { get; set; }

    public string EventName { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public decimal RegistrationFee { get; set; }

    public string? Discription { get; set; }

    public string EventPicture { get; set; } = null!;

    public long StoreID { get; set; }

    public long MemberID { get; set; }

    public string StatusCode { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;

    public virtual ICollection<RegisteredEvent> RegisteredEvent { get; set; } = new List<RegisteredEvent>();

    public virtual EventStatus StatusCodeNavigation { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
