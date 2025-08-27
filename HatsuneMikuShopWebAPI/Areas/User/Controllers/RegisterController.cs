using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LifetimeLiveHouseWebAPI.Areas.User.Controllers
{
    [Area("User")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly LifetimeLiveHouseSysDBContext _context;

        public RegisterController(LifetimeLiveHouseSysDBContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Member>> PostUserRegister(RegisterInfoDTO inputRegisterInfo)
        {
            // 檢查信箱、電話是否已被註冊
            if (await _context.User.AnyAsync(u => u.Email == inputRegisterInfo.Email))
            {
                return Unauthorized("信箱已被註冊");
            }
            else if (await _context.User.AnyAsync(u => u.PhoneNumber == inputRegisterInfo.PhoneNumber))
            {
                return Unauthorized("手機號碼已被註冊");
            }
            else
            {
                // 黑名單邏輯略過...

                // 使用轉換函式建立 User 物件
                var user = ConvertToUser(inputRegisterInfo);

                _context.User.Add(user);
                await _context.SaveChangesAsync();

                return Ok("註冊成功");
            }

        }
        // GET: api/Register
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Register/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            var member = await _context.User.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }

        // PUT: api/Register/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int id, Member member)
        {
            if (id != member.Id)
            {
                return BadRequest();
            }

            _context.Entry(member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Register
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member member)
        {
            _context.User.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMember", new { id = member.Id }, member);
        }

        // DELETE: api/Register/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.User.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.User.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
        private static Member ConvertToUser(RegisterInfoDTO u)
        {
            return new Member
            {
                Name = u.Name,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Password = BCrypt.Net.BCrypt.HashPassword(u.Password), // 使用BCrypt為密碼加鹽雜湊
                Sex = u.Sex,
                Birthday = u.Birthday
            };
        }
    }
}
