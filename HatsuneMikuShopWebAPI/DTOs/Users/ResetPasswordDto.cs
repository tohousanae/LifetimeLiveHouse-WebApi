namespace LifetimeLiveHouseWebAPI.DTOs.Users
{
    public class ResetPasswordDto
    {
        public string NewPassword { get; set; } = null!;

        public string ConfirmPassword { get; set; } = null!;
    }
}
