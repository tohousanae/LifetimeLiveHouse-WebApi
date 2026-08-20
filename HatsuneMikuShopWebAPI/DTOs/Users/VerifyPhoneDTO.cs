using System.ComponentModel.DataAnnotations;

namespace LifetimeLiveHouseWebAPI.DTOs.Users
{
    public class VerifyPhoneDTO
    {
        [Required(ErrorMessage = "會員ID不能為空")]
        [Range(1, long.MaxValue, ErrorMessage = "無效的會員ID")] // 💡 防止傳入 0 或負數的無效 ID
        public long MemberId { get; set; }

        [Required(ErrorMessage = "驗證碼不能為空")]
        [StringLength(10, ErrorMessage = "驗證碼長度異常")]
        public string Code { get; set; } = null!;
    }
}