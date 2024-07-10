using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using am3burger.Models;
using Microsoft.CodeAnalysis.Scripting;
using am3burger.DTO.Users;

namespace am3burger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Am3burgerContext _context;

        public UserController(Am3burgerContext context)
        {
            _context = context;
        }

        // 登入註冊api參考資料：https://ithelp.ithome.com.tw/articles/10337994
        // 註冊api
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDTO request)
        {
            /*
            以下使用了 **BCrypt.Net** 函式庫來將 **request.Password** 中的密碼進行哈希加密演算法處理，處理後的密碼存儲在 **passwordHash** 變數中。
            cost是指加密的複雜度，預設為11，
            cost設定越高密碼安全性就越高，但也會導致花太多效能在加密上導致效能下降，參考：https://github.com/BcryptNet/bcrypt.net
            */
            /*var cost = 11;*/
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password/*, workFactor: cost*/); 

            User user = new User
            {
                Email = request.Email,
                Password = passwordHash,
            };
            
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // 登入api(cookie based驗證)
        //[HttpPost("login")]
        //public ActionResult<User> Login(LoginDTO request)
        //{
        //    if (user.Email != request.Email) { return BadRequest("該使用者不存在"); }
        //    if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        //    {
        //        return BadRequest("信箱或密碼錯誤");
        //    }
        //    return Ok(user);
        //}
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginDTO request)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized("信箱不存在");
            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized("信箱或密碼錯誤"); // 刻意將回傳訊息設定成信箱或密碼錯誤，防止攻擊者針對密碼做攻擊測試
            }
            else
            {
                // 将用户的唯一标识符添加到Cookie中
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddYears(1);
                option.HttpOnly = true;
                option.Secure = true;
                Response.Cookies.Append("UserId", user.Id.ToString(), option);
                return Ok("登入成功");
            }
        }

        // 查詢所有會員
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.User.ToListAsync();
        }

        // 查詢個別會員
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // 修改個別會員資料
        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // 新增會員資料
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // 刪除會員資料
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
