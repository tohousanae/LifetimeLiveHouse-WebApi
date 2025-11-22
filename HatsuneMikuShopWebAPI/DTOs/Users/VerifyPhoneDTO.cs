namespace LifetimeLiveHouseWebAPI.DTOs.Users
{
    public class VerifyPhoneDTO
    {

        public long MemberId { get; set; }

        public string Code { get; set; } = null!;
    }
}
