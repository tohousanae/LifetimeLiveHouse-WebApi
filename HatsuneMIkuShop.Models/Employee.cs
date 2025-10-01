using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifetimeLiveHouse.Models;

public partial class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long EmployeeID { get; set; } // 員工編號 PK identity

    [Required]
    [MaxLength(40)]
    public string Name { get; set; } = null!; // 姓名

    public DateTime HireDate { get; set; } // 到職日

    [Required]
    [MaxLength(50)]
    public string Address { get; set; } = null!; // 地址

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
    public DateTime Birthday { get; set; } // 生日

    [Required]
    [MaxLength(20)]
    public string Tel { get; set; } = null!; // 電話

    // 以下兩個是計算欄位 (non-mapped)
    [NotMapped]
    public int Seniority // 年資
    {
        get
        {
            var now = DateTime.Now;
            int years = now.Year - HireDate.Year;
            if (now < HireDate.AddYears(years)) years--;
            return years;
        }
    }

    [NotMapped]
    public int Age // 年齡
    {
        get
        {
            var now = DateTime.Now;
            int years = now.Year - Birthday.Year;
            if (now < Birthday.AddYears(years)) years--;
            return years;
        }
    }

    [Required]
    [ForeignKey("EmployeeRole")]
    public string RoleCode { get; set; } = null!;  // 角色代碼 FK，nchar(1)

    [ForeignKey("Store")]
    public long StoreID { get; set; }  // 分店編號 FK

    public virtual Store? Store { get; set; } = null!;

    public virtual EmployeeRole? EmployeeRole { get; set; } = null!;

    public virtual ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
}
