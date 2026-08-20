using System;
using System.Collections.Generic;

namespace LifetimeLiveHouseWebAPI.Models;

public partial class RehearsalStudio
{
    public long RehearsalStudioID { get; set; }

    public string RehearsalStudioName { get; set; } = null!;

    public DateTime StartRentTime { get; set; }

    public DateTime OutRentTime { get; set; }

    public decimal RentFeePerHour { get; set; }

    public string? Discription { get; set; }

    public string RehearsalStudioPhoto { get; set; } = null!;

    public long StoreID { get; set; }

    public long? MemberID { get; set; }

    public virtual Member? Member { get; set; }

    public virtual Store Store { get; set; } = null!;
}
