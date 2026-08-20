using System;
using System.Collections.Generic;

namespace LifetimeLiveHouseWebAPI.Models;

public partial class EventStatus
{
    public string StatusCode { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<Event> Event { get; set; } = new List<Event>();

    public virtual ICollection<Live> Live { get; set; } = new List<Live>();
}
