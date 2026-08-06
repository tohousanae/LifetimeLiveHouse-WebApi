namespace LifetimeLiveHouseWebAPI.Models;

public partial class Live
{
    public long LiveID { get; set; }

    public string LiveName { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public decimal Admission { get; set; }

    public string? Discription { get; set; }

    public string LiveSong { get; set; } = null!;

    public string BandRoleID { get; set; } = null!;

    public long StoreID { get; set; }

    public long MemberID { get; set; }

    public string StatusCode { get; set; } = null!;

    public string? EventStatusStatusCode { get; set; }

    public virtual BandRole BandRole { get; set; } = null!;

    public virtual EventStatus? EventStatusStatusCodeNavigation { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
