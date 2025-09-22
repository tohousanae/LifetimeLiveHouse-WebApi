using HotelSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifetimeLiveHouse.Models;

public partial class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long EmployeeID { get; set; }

    [StringLength(40)]
    public string Name { get; set; } = null!;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
    public DateTime HireDate { get; set; }

    [StringLength(50)]
    public string Address { get; set; } = null!;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
    public DateTime Birthday { get; set; }

    [StringLength(20)]
    public string Tel { get; set; } = null!;

    [StringLength(30)]
    public string Account { get; set; } = null!;

    [StringLength(200)]
    public string Password { get; set; } = null!;

    [StringLength(1)]
    public string RoleCode { get; set; } = null!;

    [InverseProperty("Employee")]
    public virtual ICollection<Order> Order { get; set; } = new List<Order>();

    [ForeignKey("RoleCode")]
    [InverseProperty("Employee")]
    public virtual EmployeeRole RoleCodeNavigation { get; set; } = null!;

    [InverseProperty("Employee")]
    public virtual ICollection<RoomService> RoomService { get; set; } = new List<RoomService>();
}
