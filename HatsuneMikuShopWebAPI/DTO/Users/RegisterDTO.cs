namespace HatsuneMikuShopWebAPI.DTO.Users
{
    public class RegisterDTO
    {

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string PhoneNumber { get; set; }

        public required string Password { get; set; }

        public required bool Sex { get; set; }

        public DateTime Birthday { get; set; }

    }
}
