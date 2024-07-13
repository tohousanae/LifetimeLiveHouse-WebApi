using static am3burger.Models.MailSetting;

namespace am3burger.Helper
{
    public interface IMailService
    {
        Task SendEmailiAsync(MailRequest mailRequest);
    }
}
