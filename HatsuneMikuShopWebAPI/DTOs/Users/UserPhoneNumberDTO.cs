using System.ComponentModel.DataAnnotations;

namespace LifetimeLiveHouseWebAPI.DTOs.Users
{
    public class UserPhoneNumberDTO
    {
        [Required(ErrorMessage = "手機號碼不能為空")]
        [Phone(ErrorMessage = "手機號碼格式錯誤")]
        [StringLength(20, ErrorMessage = "手機號碼長度最多 20 字元")]
        public string CellphoneNumber { get; set; } = null!;
    }
}