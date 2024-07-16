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
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

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

        // 會員中心，顯示會員資料
        [HttpGet("user/{id}")]
        public async Task<UserManageDto> GetUserInfo(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null) 
            {
                return null;
            }
            UserManageDto userManageDto = new UserManageDto
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Sex = user.Sex,
                Birthday = user.Birthday,
            };
            return userManageDto;
        }

        // 登入註冊api參考資料：https://ithelp.ithome.com.tw/articles/10337994
        // 註冊api
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDTO request)
        {
            // 检查邮箱、电子邮件和电话号码是否已被注册
            if (await _context.User.AnyAsync(u => u.Email == request.Email))
            {
                return Unauthorized("信箱已被註冊");
            }
            else if (await _context.User.AnyAsync(u => u.PhoneNumber == request.PhoneNumber))
            {
                return Unauthorized("電話已被註冊");
            }
            else
            {
                /*
                1. 以下使用了 **BCrypt.Net** 函式庫來將 **request.Password** 中的密碼進行哈希加密演算法處理，處理後的密碼存儲在 **passwordHash** 變數中。
                cost是指加密的複雜度，預設為11，
                cost設定越高密碼安全性就越高，但也會導致花太多效能在加密上導致效能下降

                2. ByCrypt加密會經過加鹽處理，會將加鹽與密碼一起進行哈希加密，即便是兩個使用者使用相同的密碼其經過加密的值也不會相同，增加破解難度

                3. 參考資料：https://github.com/BcryptNet/bcrypt.net、https://ithelp.ithome.com.tw/articles/10337514
                */

                /*var cost = 11;*/
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password/*, workFactor: cost*/);

                User user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Password = passwordHash,
                    Sex = request.Sex,
                    Birthday = request.Birthday,
                    Permission = request.Permission,
                };
                 
                _context.User.Add(user);
                await _context.SaveChangesAsync();
                return Ok(user);
            }
        }

        // 登入api(cookie based驗證)
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginDTO request)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized("信箱不存在"); // 檢查輸入的信箱是否為使用者輸入的信箱
            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized("信箱或密碼錯誤"); // 刻意將回傳訊息設定成信箱或密碼錯誤，防止攻擊者針對密碼做攻擊測試
            }
            else
            {
                // 将用户的唯一标识符添加到Cookie中
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddYears(1); // cookie過期時間設定
                option.HttpOnly = true;
                option.Secure = true;
                Response.Cookies.Append("UserId", user.Id.ToString(), option);
                return Ok("登入成功");
            }
        }

        // 生成忘記密碼的token，作為一次性連結使用
        [HttpPost("forgetpasswordEmailSendTokenGen")]
        public async Task<ActionResult<User>> ForgetpasswordEmailSendTokenGen(ForgetPasswordDto request)
        {
            /* 忘記密碼設計 
                
            start 使用者輸入信箱(需在前端設計
            -->驗證是否為註冊信箱
            -->是(若為否則告知使用者信箱不存在)
            -->基於安全性考量，將token加密儲存到cookie內並設定option.Secure = true;防止前端用js直接讀取cookie，並設定cookie(token的失效時間)
            -->發送忘記密碼重設信件，token加入到忘記密碼頁面的一次性連結內
            -->當使用者完成密碼重設，或超過token保存時效未重設密碼時，連結失效
            -->通知使用者密碼已完成重設

             */
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized("信箱不存在"); // 檢查輸入的信箱是否為使用者輸入的信箱
            }
            else
            {
                /* 生成token參考資料：
                 * 1. https://blog.csdn.net/m0_38013946/article/details/134849150
                 * 2. https://hackercat.org/diy-tools/generate-random-password-from-command-line
                 */
                Guid guid = Guid.NewGuid(); // 生成GUID token
                string token = guid.ToString("N");

                // 將token雜湊加密
                string tokenHash = BCrypt.Net.BCrypt.HashPassword(token);

                // 將token儲存到cookie中，並設置該cookie失效時間
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddMinutes(30); // 設定token的失效時間，作為忘記密碼連結失效時間
                option.HttpOnly = true;
                option.Secure = true;
                Response.Cookies.Append("forgetPwdToken", tokenHash, option);
                Response.Cookies.Append("InputEmail",request.Email.ToString(), option);

                return Ok(tokenHash);
            }
        }

        // 使用者點擊忘記密碼連結時，透過驗證token來驗證連結是否有效
        [HttpPost("checkforgetPwdlink")]
        public IActionResult CheckforgetPwdlink()
        {
            string? cookieValue = Request.Cookies["forgetPwdToken"];

            if (cookieValue != null)
            {
                return Ok(cookieValue);
            }
            else
            {
                return Unauthorized("你點擊的連結不存在");
            }
        }

        // 使用者完成修改密碼操作後，清除儲存忘記密碼token的cookie，並且清除使用者Id的cookie讓使用者在所有裝置上登出
        [HttpPatch("modifyPwd")]
        public async Task<IActionResult> ModifyPwd(ForgetPasswordModifyPasswordDto request)
        {
            // 從cookie取得使用者輸入的email
            string? inputemail = System.Net.WebUtility.UrlDecode(Request.Cookies["InputEmail"]); // 將URL編碼的字串轉換回原始的字串
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == inputemail); 

            if (user == null) 
            {
                return BadRequest("使用者紀錄不存在");
            }
            else
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password); // 修改的密碼雜湊加密後儲存
                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    // 儲存修改密碼
                    await _context.SaveChangesAsync();
                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(-1);
                    option.HttpOnly = true;
                    option.Secure = true;
                    Response.Cookies.Append("forgetPwdToken", "", option);
                    Response.Cookies.Append("UserId", "", option);
                    Response.Cookies.Append("InputEmail", "", option);
                }
                catch (DbUpdateConcurrencyException) 
                { 
                    throw;
                }
                return Ok("成功重設密碼，請重新登入");
            }
        }

        // 讀取cookie驗證是否登入
        [HttpPost("loginCkeck")]
        public IActionResult CheckLoginStatus()
        {
            // 获取请求中的 cookie 数据
            string? cookieValue = Request.Cookies["UserId"];

            // 检查是否存在有效的登录会话或认证凭据
            if (cookieValue != null)
            {
                // 用户已登录，返回成功的响应
                return Ok(cookieValue);
            }
            else
            {
                // 用户未登录，返回错误的响应
                return Unauthorized("使用者未登入");
            }
        }

        // 登出，清除cookie
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // 清除用户的身份验证凭证（例如，清除存储在 cookie 中的身份验证令牌）
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(-1);
            option.HttpOnly = true;
            option.Secure = true;
            Response.Cookies.Append("UserId", "", option);
            return Ok("登出成功");
            // 返回成功响应
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
