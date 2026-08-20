using System;
using System.Collections.Generic;

namespace LifetimeLiveHouseWebAPI.Models;

public partial class News
{
    public long NewsID { get; set; }

    public string NewsTitle { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string? Description { get; set; }

    public string Picture { get; set; } = null!;

    public DateTime PostDate { get; set; }
}
