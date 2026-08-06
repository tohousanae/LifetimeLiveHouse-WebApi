namespace LifetimeLiveHouseWebAPI.Models;

public partial class LoginRecord
{
    public long RecordID { get; set; }

    public string? Record { get; set; }

    public DateTime LoginDate { get; set; }

    public long ContinuousLoginDate { get; set; }

    public long MemberID { get; set; }

    public virtual Member Member { get; set; } = null!;
}
