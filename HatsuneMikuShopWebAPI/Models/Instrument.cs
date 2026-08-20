using System;
using System.Collections.Generic;

namespace LifetimeLiveHouseWebAPI.Models;

public partial class Instrument
{
    public long InstrumentID { get; set; }

    public string InstrumentName { get; set; } = null!;

    public DateTime RentTime { get; set; }

    public DateTime OutRentTime { get; set; }

    public decimal RentFeePerHour { get; set; }

    public string? Discription { get; set; }

    public string InstrumentPhoto { get; set; } = null!;

    public long StoreID { get; set; }

    public long? MemberID { get; set; }

    public virtual Member? Member { get; set; }

    public virtual Store Store { get; set; } = null!;
}
