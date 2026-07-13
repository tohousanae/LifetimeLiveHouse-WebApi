using System;
using System.Collections.Generic;

namespace LifetimeLiveHouseWebAPI.Models;

public partial class MemberStatus
{
    public string StatusCode { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<Member> Member { get; set; } = new List<Member>();
}
