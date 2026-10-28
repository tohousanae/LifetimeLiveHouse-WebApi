using System.ComponentModel.DataAnnotations;

namespace LifetimeLiveHouseWebAPI.DTOs.Users
{
    public class VerifyPhoneDTO
    {
        [Required(ErrorMessage = "驗證碼不能為空")]
        [StringLength(10, ErrorMessage = "驗證碼長度異常")]
        public string Code { get; set; } = null!;
    }
}