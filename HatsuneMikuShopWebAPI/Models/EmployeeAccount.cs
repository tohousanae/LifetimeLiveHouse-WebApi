namespace LifetimeLiveHouseWebAPI.Models;

public partial class EmployeeAccount
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public long EmployeeID { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
