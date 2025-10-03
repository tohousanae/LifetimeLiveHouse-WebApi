using System.ComponentModel.DataAnnotations;
namespace LifetimeLiveHouse.Models;

public partial class EmployeeRole
{
    [Key]
    [StringLength(1)]
    [Required]
    public string RoleCode { get; set; } = null!;

    [StringLength(15)]
    [Required]
    public string RoleName { get; set; } = null!;

    // 集合導覽是 .NET 集合類型的實例;也就是說，任何實作 ICollection<T>的類型。 集合包含相關實體類型的實例，其中可以有任意數目。 它們代表了一對多和多對多關係的“多”端。 
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
