namespace LifetimeLiveHouseWebAPI.Models;

public partial class Employee
{
    public long EmployeeID { get; set; }

    public string Name { get; set; } = null!;

    public DateTime HireDate { get; set; }

    public string Address { get; set; } = null!;

    public DateTime Birthday { get; set; }

    public string Tel { get; set; } = null!;

    public string RoleCode { get; set; } = null!;

    public long StoreID { get; set; }

    public virtual ICollection<AttendanceRecord> AttendanceRecord { get; set; } = new List<AttendanceRecord>();

    public virtual EmployeeAccount? EmployeeAccount { get; set; }

    public virtual ICollection<Order> Order { get; set; } = new List<Order>();

    public virtual EmployeeRole RoleCodeNavigation { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
