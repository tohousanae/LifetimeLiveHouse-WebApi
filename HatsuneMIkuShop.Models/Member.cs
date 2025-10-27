using LifetimeLiveHouse.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 會員資料表
public partial class Member
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
    public DateTime Birthday { get; set; }   // 生日

    [Column(TypeName = "money")]
    [Range(0, double.MaxValue)]
    public decimal Cash { get; set; } = 0;   // 儲值金額 (default 0)

    [NotMapped] // 年齡計算屬性，不會在資料庫建立欄位
    public int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - Birthday.Year;

            // 若今年生日還沒到，年齡要 -1
            if (Birthday.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }

    [StringLength(20)]
    public string CellphoneNumber { get; set; } = null!;   // 手機號碼

    [Range(0, int.MaxValue)]
    public int MemberPoint { get; set; } = 0;   // 回饋點數

    [ForeignKey("MemberStatus")]
    [StringLength(1)]
    [Column(TypeName = "nchar")]
    [Required]        // 確保在 C#／驗證層面必填
    public string StatusCode { get; set; } = "0";  // 預設值為 "0"

    // 導覽屬性
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual MemberStatus MemberStatus { get; set; } = null!;

    public virtual MemberEmailVerificationStatus MemberEmailVerificationStatus { get; set; } = null!;

    public virtual MemberPhoneVerificationStatus MemberPhoneVerificationStatus { get; set; } = null!;

    public virtual Seat? Seat { get; set; }

    public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Instrument>? Instruments { get; set; } = new List<Instrument>();

    public virtual ICollection<RehearsalStudio>? RehearsalStudios { get; set; } = new List<RehearsalStudio>();

    public virtual ICollection<Live> Lives { get; set; } = new List<Live>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<LoginRecord> LoginRecords { get; set; } = new List<LoginRecord>();

    public virtual MemberHeadPicture? MemberHeadPicture { get; set; }

    public virtual ICollection<MemberPicture> MemberPictures { get; set; } = new List<MemberPicture>();

    public virtual ICollection<RegisteredEvent> RegisteredEvents { get; set; } = new List<RegisteredEvent>();

    public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();

}