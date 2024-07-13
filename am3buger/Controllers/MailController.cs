using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using am3burger.Helper;
using static am3burger.Models.MailSetting;

namespace am3burger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        // 套用Mail的Method
        private readonly IMailService _mailService;
        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail(/*[FromForm] */MailRequest request)// 若有傳檔案要使用`[FromForm]`
        {
            try
            {
                await _mailService.SendEmailiAsync(request);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
