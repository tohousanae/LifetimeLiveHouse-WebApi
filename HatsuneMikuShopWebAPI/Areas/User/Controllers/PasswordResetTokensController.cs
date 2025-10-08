//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using LifetimeLiveHouse.Access.Data;
//using LifetimeLiveHouse.Models;
//using NETCore.MailKit.Core;

//namespace LifetimeLiveHouseWebAPI.Areas.User.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PasswordResetTokensController : ControllerBase
//    {
//        private readonly LifetimeLiveHouseSysDBContext _context;

//        public PasswordResetTokensController(LifetimeLiveHouseSysDBContext context)
//        {
//            _context = context;
//        }

//        [Route("api/[controller]")]
//        [ApiController]
//        public class PasswordResetTokensController : ControllerBase
//        {
//            private readonly LifetimeLiveHouseSysDBContext _context;
//            private readonly IEmailService _emailService;
//            private readonly string _frontendBaseUrl;

//            public PasswordResetTokensController(
//                LifetimeLiveHouseSysDBContext context,
//                IEmailService emailService,
//                string frontendBaseUrl)
//            {
//                _context = context;
//                _emailService = emailService;
//                _frontendBaseUrl = frontendBaseUrl;
//            }

//            [HttpPost("forgot-password")]
//            public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
//            {
//                if (!ModelState.IsValid)
//                    return BadRequest(ModelState);

//                var user = await _context.Users
//                    .SingleOrDefaultAsync(u => u.Email == dto.Email);

//                var responseMsg = "如果該信箱有註冊，我們已發送重設密碼信件，請檢查信件。";

//                if (user != null)
//                {
//                    string token = GenerateSecureToken();
//                    string hash = HashToken(token);

//                    var prt = new PasswordResetToken
//                    {
//                        UserId = user.Id,
//                        TokenHash = hash,
//                        CreatedAt = DateTime.UtcNow,
//                        ExpiresAt = DateTime.UtcNow.AddHours(1),
//                        Used = false
//                    };

//                    _context.PasswordResetTokens.Add(prt);
//                    await _context.SaveChangesAsync();

//                    string resetLink = $"{_frontendBaseUrl}/reset-password?token={Uri.EscapeDataString(token)}";
//                    await _emailService.SendPasswordResetAsync(user.Email, resetLink);
//                }

//                return Ok(new { message = responseMsg });
//            }

//            [HttpPost("reset-password")]
//            public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
//            {
//                if (!ModelState.IsValid)
//                    return BadRequest(ModelState);

//                if (dto.NewPassword != dto.ConfirmPassword)
//                    return BadRequest(new { message = "密碼與確認密碼不一致。" });

//                string tokenHash = HashToken(dto.Token);

//                var prt = await _context.PasswordResetTokens
//                    .Where(t => t.TokenHash == tokenHash
//                                && !t.Used
//                                && t.ExpiresAt > DateTime.UtcNow)
//                    .FirstOrDefaultAsync();

//                if (prt == null)
//                    return BadRequest(new { message = "重設密碼 token 無效或已過期。" });

//                var user = await _context.Users.FindAsync(prt.UserId);
//                if (user == null)
//                    return BadRequest(new { message = "使用者不存在。" });

//                user.PasswordHash = HashPassword(dto.NewPassword);

//                prt.Used = true;
//                prt.UsedAt = DateTime.UtcNow;

//                await _context.SaveChangesAsync();

//                return Ok(new { message = "密碼已重設成功。" });
//            }
//        }
//}
