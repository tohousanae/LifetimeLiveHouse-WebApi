namespace LifetimeLiveHouseWebAPI.Models;

public partial class RegisteredEvent
{
    public long RecordID { get; set; }

    public long EventID { get; set; }

    public long MemberID { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
