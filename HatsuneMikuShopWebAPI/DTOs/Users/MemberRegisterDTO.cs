using System.ComponentModel.DataAnnotations;

namespace LifetimeLiveHouseWebAPI.DTOs.Users
{
    public class MemberRegisterDTO
    {
        [Required(ErrorMessage = "會員名稱不能為空")]
        [StringLength(40, MinimumLength = 1, ErrorMessage = "會員名稱長度必須在 1 到 40 字元之間")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "信箱不能為空")]
        [EmailAddress(ErrorMessage = "信箱格式錯誤")]
        [StringLength(30, ErrorMessage = "信箱長度最多 30 字元")] // 💡 資料字典定義 Email 為 nvarchar(30)
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "密碼不能為空")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "生日不能為空")]
        public DateTime Birthday { get; set; }

        // ❌ 已根據規格移除 Sex 欄位
    }
}