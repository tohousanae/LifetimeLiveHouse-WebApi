namespace LifetimeLiveHouseWebAPI.Models;

public partial class BandRole
{
    public string BandRoleID { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Live> Live { get; set; } = new List<Live>();
}
