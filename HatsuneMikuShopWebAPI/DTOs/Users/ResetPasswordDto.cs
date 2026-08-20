using System.ComponentModel.DataAnnotations;

namespace LifetimeLiveHouseWebAPI.DTOs.Users
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "新密碼不能為空")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\w\d\s:])([^\s]){8,16}$",
            ErrorMessage = "密碼必須包含至少1個數字、1個大寫字母、1個小寫字母和1個特殊字元，且長度為8-16碼")]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "確認密碼不能為空")]
        [Compare("NewPassword", ErrorMessage = "新密碼與確認密碼不一致")] // 💡 自動比對欄位
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "驗證碼不能為空")]
        public string InputToken { get; set; } = null!;
    }
}