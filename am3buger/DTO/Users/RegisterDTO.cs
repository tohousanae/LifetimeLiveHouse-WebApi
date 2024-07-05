namespace am3burger.DTO.Users
{
    public class RegisterDTO
    {
        // 會員Id
        public int Id { get; set; }

        // 姓名
        public string? Name { get; set; }

        // 信箱
        public string? Email { get; set; }

        // 手機號碼
        public string? PhoneNumber { get; set; }

        // 密碼
        public string? Password { get; set; }

        // 性別
        public string? Sex { get; set; }

        // 生日
        public DateTime Birthday { get; set; }

        // 權限
        public int Permission { get; set; }
    }
}
