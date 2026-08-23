using System;
using System.Collections.Generic;

namespace LifetimeLiveHouse.Models;

public partial class MemberAccount
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public long MemberID { get; set; }

    public virtual Member Member { get; set; } = null!;
}
