//using LifetimeLiveHouse.Access.Data;
//using LifetimeLiveHouse.Models;
//using LifetimeLiveHouseWebAPI.DTOs.Users;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace LifetimeLiveHouseWebAPI.Areas.User.Controllers
//{
//    [Area("User")]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RegisterController : ControllerBase
//    {
//        private readonly LifetimeLiveHouseSysDBContext _context;

//        public RegisterController(LifetimeLiveHouseSysDBContext context)
//        {
//            _context = context;
//        }

//        [HttpPost("register-full")]
//        public async Task<ActionResult> PostUserFullRegister(MemberRegisterDTO input)
//        {
//            // 檢查 Email / 手機
//            if (await _context.MemberAccount.AnyAsync(u => u.Email == input.Email))
//                return Unauthorized("信箱已被註冊");
//            if (await _context.Member.AnyAsync(u => u.CellphoneNumber == input.CellphoneNumber))
//                return Unauthorized("手機號碼已被註冊");

//            var user = ConvertToUser(new MemberRegisterDTO
//            {
//                Name = input.Name,
//                Password = input.Password,
//                Birthday = input.Birthday,
//                Sex = input.Sex
//            });

//            var userAccount = new MemberAccount
//            {
//                Email = input.Email,
//                Password = user.Password,
//                Member = user
//            };

//            _context.Member.Add(user);
//            _context.MemberAccount.Add(userAccount);

//            await _context.SaveChangesAsync();

//            return Ok("完整會員註冊成功");
//        }
        
//    }
//}
