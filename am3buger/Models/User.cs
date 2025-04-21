using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace am3burger.Models
{
    // 不常變動的資料增加索引，提高查詢速度
    [Index(nameof(Email), IsUnique = true, Name = "IX_User_Email")]
    [Index(nameof(PhoneNumber), IsUnique = true, Name = "IX_User_PhoneNumber")]
    [Index(nameof(Sex), IsUnique = true, Name = "IX_User_Sex")]
    [Index(nameof(Birthday), IsUnique = true, Name = "IX_User_Birthday")]
    [Index(nameof(Identity), IsUnique = true, Name = "IX_User_Identity")]
    [Index(nameof(PhoneValidation), IsUnique = true, Name = "IX_User_PhoneValidation")]
    [Index(nameof(EmailValidation), IsUnique = true, Name = "IX_User_EmailValidation")]
    // 會員資料表
    public class User
    {
        [Key]
        [Display(Name = "會員Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // 會員Id  
        public int Id { get; set; }

        [Display(Name = "會員名稱")]
        [StringLength(10)]
        // 姓名
        public string? Name { get; set; }

        [Display(Name = "信箱")]
        [StringLength(50)]
        // 信箱
        public string? Email { get; set; }

        [Display(Name = "手機號碼")]
        [StringLength(10)]
        // 手機號碼
        public string? PhoneNumber { get; set; }

        [Display(Name = "密碼")]
        [StringLength(4000)]
        // 密碼
        public string? Password { get; set; }

        [Display(Name = "性別")]
        [StringLength(5)]
        // 性別
        public string? Sex { get; set; }

        [Display(Name = "生日")]
        // 生日
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? Birthday { get; set; }

        [Display(Name = "身分")]
        [MaxLength(5)]
        // 權限
        public string? Identity { get; set; }

        [Display(Name = "手機驗證狀態")]
        // 手機號碼
        public bool? PhoneValidation { get; set; }

        [Display(Name = "信箱驗證狀態")]
        // 手機號碼
        public bool? EmailValidation { get; set; }
    }
}
