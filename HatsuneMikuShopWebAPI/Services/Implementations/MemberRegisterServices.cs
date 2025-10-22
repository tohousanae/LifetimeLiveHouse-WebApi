using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LifetimeLiveHouseWebAPI.Services.Implementations
{
    public class MemberRegisterServices : IMemberRegisterServices
    {
        private readonly LifetimeLiveHouseSysDBContext _context;

        public MemberRegisterServices(
            LifetimeLiveHouseSysDBContext context)
        {
            _context = context;
        }
        public async Task<ActionResult<string>> MemberRegisterAsync(MemberRegisterDTO dto)
        {
            // 檢查 Email / 手機
            if (await _context.MemberAccount.AnyAsync(u => u.Email == dto.Email))
                return "信箱已被註冊";

            if (await _context.Member.AnyAsync(u => u.CellphoneNumber == dto.CellphoneNumber))
                return "手機號碼已被註冊";

            var user = ConvertToUser(new MemberRegisterDTO
            {
                Name = dto.Name,
                Password = dto.Password,
                Birthday = dto.Birthday,
                CellphoneNumber = dto.CellphoneNumber,
            });

            var userAccount = new MemberAccount
            {
                Email = dto.Email,
                Password = user.Password,
                Member = user
            };

            _context.Member.Add(user);
            _context.MemberAccount.Add(userAccount);

            await _context.SaveChangesAsync();

            return "完整會員註冊成功";
        }
    }
}
