using System.ComponentModel.DataAnnotations;

namespace LifetimeLiveHouseWebAPI.DTOs.Users
{
    public class MemberUpdateDTO
    {
        [Required(ErrorMessage = "會員名稱不能為空")]
        [StringLength(40, MinimumLength = 1, ErrorMessage = "會員名稱長度必須在 1 到 40 字元之間")]
        public string Name { get; set; } = null!;

        [Phone(ErrorMessage = "手機號碼格式錯誤")]
        [StringLength(20, ErrorMessage = "手機號碼長度最多 20 字元")]
        public string? CellphoneNumber { get; set; }

        public DateTime? Birthday { get; set; }
    }
}