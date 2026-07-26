namespace LifetimeLiveHouseWebAPI.Models;

public partial class Notification
{
    public long NotificationID { get; set; }

    public string? Description { get; set; }

    public bool Readed { get; set; }

    public long MemberID { get; set; }

    public virtual Member Member { get; set; } = null!;
}
