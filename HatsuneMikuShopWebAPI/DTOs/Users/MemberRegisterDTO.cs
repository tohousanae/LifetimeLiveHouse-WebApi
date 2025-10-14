
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifetimeLiveHouseWebAPI.DTOs.Users
{
    public class MemberRegisterDTO
    {
        [StringLength(40)]
        public string Name { get; set; } = null!;    // 暱稱


        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        public DateTime Birthday { get; set; }   // 生日

        [StringLength(20)]
        public string CellphoneNumber { get; set; } = null!;   // 手機號碼

        [ForeignKey("MemberStatus")]
        [StringLength(1)]
        [Column(TypeName = "nchar")]
        public string StatusCode { get; set; } = "0"; // 狀態編號 (FK)

        [Key]
        [StringLength(30)]
        public string Email { get; set; } = null!;

        [StringLength(200)]
        public string Password { get; set; } = null!;

    }
}
