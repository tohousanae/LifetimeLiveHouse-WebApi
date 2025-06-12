namespace HatsuneMikuMusicShop_MVC.DTO.Users
// dto，
{
    public class UserDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Sex { get; set; }
        public DateTime Birthday { get; set; }
        public int? MikuPoint { get; set; }
    }
}
