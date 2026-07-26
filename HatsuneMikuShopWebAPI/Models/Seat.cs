namespace LifetimeLiveHouseWebAPI.Models;

public partial class Seat
{
    public long SeatID { get; set; }

    public long MemberID { get; set; }

    public long StoreID { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
