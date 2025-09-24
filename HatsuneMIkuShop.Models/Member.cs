using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace LifetimeLiveHouse.Models
{
    // 會員資料表
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MemberID { get; set; }   // 主鍵 P.K

        [StringLength(40)]
        public string Name { get; set; } = null!;    // 暱稱

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        [HiddenInput]
        public DateTime CreatedDate { get; set; } = DateTime.Now; // 建立日期

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        public DateTime? Birthday { get; set; }   // 生日

        [Column(TypeName = "money")]
        [Range(0, double.MaxValue)]
        public decimal Cash { get; set; } = 0;   // 儲值金額 (default 0)

        [NotMapped]  // 不存入資料庫 (從生日計算)
        public int Age
        {
            get
            {
                if (Birthday == null) return 0;
                var today = DateTime.Today;
                var age = today.Year - Birthday.Value.Year;
                if (Birthday.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        [StringLength(20)]
        public string CellphoneNumber { get; set; } = null!;   // 手機號碼

        [Range(0, int.MaxValue)]
        public int MemberPoint { get; set; } = 0;   // 回饋點數

        [ForeignKey("MemberPicture")]
        public string? Picture { get; set; }   // 頭像圖片名稱 (FK)

        [ForeignKey("MemberStatus")]
        public string StatusCode { get; set; } = null!; // 狀態編號 (FK)

        // 導覽屬性
        public virtual MemberStatus? MemberStatus { get; set; }

        public virtual MemberPicture? MemberPicture { get; set; }
    }
}
