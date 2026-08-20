using System;
using System.Collections.Generic;

namespace LifetimeLiveHouseWebAPI.Models;

public partial class Store
{
    public long StoreID { get; set; }

    public string StoreName { get; set; } = null!;

    public decimal RentFeePerHour { get; set; }

    public string sTel { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Discription { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Region { get; set; } = null!;

    public virtual ICollection<Employee> Employee { get; set; } = new List<Employee>();

    public virtual ICollection<Event> Event { get; set; } = new List<Event>();

    public virtual ICollection<Instrument> Instrument { get; set; } = new List<Instrument>();

    public virtual ICollection<Live> Live { get; set; } = new List<Live>();

    public virtual ICollection<RehearsalStudio> RehearsalStudio { get; set; } = new List<RehearsalStudio>();

    public virtual ICollection<Seat> Seat { get; set; } = new List<Seat>();
}
