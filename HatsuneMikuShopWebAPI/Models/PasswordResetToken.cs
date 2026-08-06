namespace LifetimeLiveHouseWebAPI.Models;

public partial class PasswordResetToken
{
    public long Id { get; set; }

    public long MemberID { get; set; }

    public string TokenHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool Used { get; set; }

    public DateTime? UsedAt { get; set; }

    public virtual Member Member { get; set; } = null!;
}
