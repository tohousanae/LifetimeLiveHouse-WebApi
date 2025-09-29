using LifetimeLiveHouse.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class AttendanceRecord
{
    [Key]
    [Required]
    public long AttendanceID { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
    public DateTime? PunchInTime { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
    public DateTime? PunchOutTime { get; set; }

    [ForeignKey("Employee")]
    [Required]
    public long EmployeeID { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
