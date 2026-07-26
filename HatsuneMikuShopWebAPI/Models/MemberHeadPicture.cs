namespace LifetimeLiveHouseWebAPI.Models;

public partial class MemberHeadPicture
{
    public string Picture { get; set; } = null!;

    public long MemberID { get; set; }

    public virtual Member Member { get; set; } = null!;
}
