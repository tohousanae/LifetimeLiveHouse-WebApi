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

        public string? Identity { get; set; }

        public bool? PhoneValidation { get; set; }

        public bool? EmailValidation { get; set; }

    }
}
