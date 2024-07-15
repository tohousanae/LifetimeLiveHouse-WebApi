namespace am3burger.Models
{
    public class MailSetting
    {
        public class MailRequest
        {
            public required string ToEmail { get; set; }
            public required string Subject { get; set; }
            public required string Body { get; set; }
            //public required List<IFormFile> Attachments { get; set; }
        }
        public class MailSettings
        {
            public required string Mail { get; set; }
            public required string DisplayName { get; set; }
            public required string Password { get; set; }
            public required string Host { get; set; }
            public required int Port { get; set; }
        }
    }
}
