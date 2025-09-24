using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouse.Models
{
    // 會員資料表
    public class AttendanceRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AttendanceRecordID { get; set; }   // 主鍵 P.K

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        public DateTime PunchInTime { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        public DateTime? PunchOutTime { get; set; } = null!;

        [ForeignKey("MemberPicture")]
        public string? Picture { get; set; }   // 頭像圖片名稱 (FK)

        [ForeignKey("MemberStatus")]
        public string StatusCode { get; set; } = null!; // 狀態編號 (FK)

        // 導覽屬性
        public virtual MemberStatus? MemberStatus { get; set; }

        public virtual MemberPicture? MemberPicture { get; set; }
    }
}
