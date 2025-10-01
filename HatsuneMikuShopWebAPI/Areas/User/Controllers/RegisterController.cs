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

//        [HttpPost("register")]
//        public async Task<ActionResult<Member>> PostUserRegister(RegisterDTO inputRegisterInfo)
//        {
//            // 檢查信箱、電話是否已被註冊
//            if (await _context.MemberAccount.AnyAsync(u => u.Email == inputRegisterInfo.Email))
//            {
//                return Unauthorized("信箱已被註冊");
//            }
//            else if (await _context.Member.AnyAsync(u => u.CellPhoneNumber == inputRegisterInfo.PhoneNumber))
//            {
//                return Unauthorized("手機號碼已被註冊");
//            }
//            else
//            {
//                // 黑名單邏輯略過...

//                // 使用轉換函式建立 User 物件
//                var user = ConvertToUser(inputRegisterInfo);

//                _context.User.Add(user);
//                await _context.SaveChangesAsync();

//                return Ok("註冊成功");
//            }

//        }
//        private bool MemberExists(int id)
//        {
//            return _context.Member.Any(e => e.MemberID == id);
//        }
//        private static Member ConvertToUser(RegisterDTO u)
//        {
//            return new Member
//            {
//                Name = u.Name,
//                Email = u.Email,
//                PhoneNumber = u.PhoneNumber,
//                Password = BCrypt.Net.BCrypt.HashPassword(u.Password), // 使用BCrypt為密碼加鹽雜湊
//                Sex = u.Sex,
//                Birthday = u.Birthday
//            };
//        }
//    }
//}
