using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HatsuneMikuMusicShop_MVC.Models
{
    [Index(nameof(Email), IsUnique = true, Name = "IX_User_Email")]
    [Index(nameof(PhoneNumber), IsUnique = true, Name = "IX_User_PhoneNumber")]
    [Index(nameof(Name), IsUnique = false, Name = "IX_User_Name")]
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
        public string Name { get; set; } = null!;

        [Display(Name = "信箱")]
        [StringLength(50)]
        // 信箱
        public string Email { get; set; } = null!;

        [Display(Name = "手機號碼")]
        [StringLength(10)]
        // 手機號碼
        public string PhoneNumber { get; set; } = null!;

        [Display(Name = "密碼")]
        [StringLength(int.MaxValue)]
        // 密碼
        public string Password { get; set; } = null!;

        [Display(Name = "性別")]
        // 性別
        public bool Sex { get; set; }

        [Display(Name = "生日")]
        // 生日
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; } = DateTime.Now;

        [Display(Name = "MikuMiku點數")]
        // 會員Id  
        public int MikuMikuPoint { get; set; } = 0;
    }
}
