namespace LifetimeLiveHouseWebAPI.Models;

public partial class AttendanceRecord
{
    public long AttendanceID { get; set; }

    public DateTime? PunchInTime { get; set; }

    public DateTime? PunchOutTime { get; set; }

    public long EmployeeID { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
