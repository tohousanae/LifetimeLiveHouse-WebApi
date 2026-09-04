using System;
using System.Collections.Generic;

namespace LifetimeLiveHouse.Models;

public partial class EmployeeRole
{
    public string RoleCode { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Employee> Employee { get; set; } = new List<Employee>();
}
