using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace am3burger.Models
{
    // 會員資料表
    public class User
    {
        [Key]
        [Display(Name = "會員Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // 會員Id  
        public int Id { get; set; }

        [Display(Name = "姓名")]
        [StringLength(50)]
        // 姓名
        public string? Name { get; set; }

        [Display(Name = "信箱")]
        [StringLength(50)]
        // 信箱
        public string? Email { get; set; }

        [Display(Name = "手機號碼")]
        [StringLength(50)]
        // 手機號碼
        public string? PhoneNumber { get; set; }

        [Display(Name = "密碼")]
        [StringLength(4000)]
        // 密碼
        public string? Password { get; set; }

        [Display(Name = "性別")]
        [StringLength(50)]
        // 性別
        public string? Sex { get; set; }

        [Display(Name = "生日")]
        // 生日
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }

        [Display(Name = "權限")]
        // 權限
        public int Permission { get; set; }
    }
}
