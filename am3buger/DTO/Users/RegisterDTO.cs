namespace am3burger.DTO.Users
{
    public class RegisterDTO
    {

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Password { get; set; }

        public string? Sex { get; set; }

        public DateTime Birthday { get; set; }

        public int Permission { get; set; }
    }
}
