namespace LifetimeLiveHouseWebAPI.Models;

public partial class MemberPhoneVerificationStatus
{
    public long MemberID { get; set; }

    public bool IsPhoneVerified { get; set; }

    public virtual Member Member { get; set; } = null!;
}
