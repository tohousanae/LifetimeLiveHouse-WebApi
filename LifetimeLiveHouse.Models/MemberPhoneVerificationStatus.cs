using System;
using System.Collections.Generic;

namespace LifetimeLiveHouse.Models;

public partial class MemberPhoneVerificationStatus
{
    public long MemberID { get; set; }

    public bool IsPhoneVerified { get; set; }

    public virtual Member Member { get; set; } = null!;
}
