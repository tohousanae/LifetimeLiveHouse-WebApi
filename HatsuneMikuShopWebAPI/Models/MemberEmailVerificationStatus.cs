using System;
using System.Collections.Generic;

namespace LifetimeLiveHouseWebAPI.Models;

public partial class MemberEmailVerificationStatus
{
    public long MemberID { get; set; }

    public bool IsEmailVerified { get; set; }

    public string? EmailVerificationTokenHash { get; set; }

    public DateTime? EmailVerificationTokenExpiry { get; set; }

    public virtual Member Member { get; set; } = null!;
}
