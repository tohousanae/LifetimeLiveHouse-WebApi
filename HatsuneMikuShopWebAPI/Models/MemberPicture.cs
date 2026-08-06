namespace LifetimeLiveHouseWebAPI.Models;

public partial class MemberPicture
{
    public string Picture { get; set; } = null!;

    public long MemberID { get; set; }

    public virtual Member Member { get; set; } = null!;
}
