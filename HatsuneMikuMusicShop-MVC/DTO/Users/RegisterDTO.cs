namespace am3burger.DTO.User
{
    public class RegisterDTO
    {

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string PhoneNumber { get; set; }

        public required string Password { get; set; }

        public required string Sex { get; set; }

        public DateTime Birthday { get; set; }

    }
}
