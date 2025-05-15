using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using am3burger.Models;
using am3burger.DTO.Users;
using am3burger.DTO.User;

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
        public async Task<ActionResult<UserDto>> GetUserInfo(int inputId)
        {
            var user = await _context.User.FindAsync(inputId);
            if (user == null) 
            {
                return NotFound("找不到此會員");
            }
            UserDto userManageDto = new UserDto
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Sex = user.Sex,
                Birthday = (DateTime)user?.Birthday,
                MikuPoint = user.MikuPoint,
            };
            return userManageDto;
        }

        // 修改 Register 方法中的以下代碼
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDTO input)
        {
            // 檢查信箱、電話是否已被註冊
            if (await _context.User.AnyAsync(u => u.Email == input.Email))
            {
                return Unauthorized("信箱已被註冊");
            }
            else if (await _context.User.AnyAsync(u => u.PhoneNumber == input.PhoneNumber))
            {
                return Unauthorized("電話已被註冊");
            }
            else
            {
                // 密碼加密與加鹽處理
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(input.Password);

                // 將 RegisterDTO 轉換為 User 模型
                User user = new()
                {
                    Name = input.Name,
                    Email = input.Email,
                    PhoneNumber = input.PhoneNumber,
                    Password = passwordHash,
                    Sex = input.Sex,
                    Birthday = input.Birthday,
                };

                _context.User.Add(user);
                await _context.SaveChangesAsync();
                return Ok("註冊成功");
            }
        }

        // 登入api(cookie based驗證)
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginDTO input)
        {
            // 檢查輸入的信箱是否為使用者輸入的信箱
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == input.Email); 

            if (user == null)
            {
                return Unauthorized("電子郵件或密碼不存在"); 
            }

            // 檢查輸入的密碼是否為使用者輸入的密碼
            if (!BCrypt.Net.BCrypt.Verify(input.Password, user.Password))
            {
                return Unauthorized("電子郵件或密碼不存在"); // 刻意將回傳訊息設定成信箱或密碼錯誤，防止攻擊者針對密碼做攻擊測試
            }
            else
            {
                // 将用户的唯一标识符添加到Cookie中
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddMonths(6); // cookie過期時間設定
                option.HttpOnly = true; // 強制使用https存取cookie 
                option.Secure = true; // 禁用js讀取cookie防止xss攻擊
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

                /* cookie相關參考：
                 * 1. 網站安全🔒 再探同源政策，談 SameSite 設定對 Cookie 的影響與注意事項 https://medium.com/%E7%A8%8B%E5%BC%8F%E7%8C%BF%E5%90%83%E9%A6%99%E8%95%89/%E5%86%8D%E6%8E%A2%E5%90%8C%E6%BA%90%E6%94%BF%E7%AD%96-%E8%AB%87-samesite-%E8%A8%AD%E5%AE%9A%E5%B0%8D-cookie-%E7%9A%84%E5%BD%B1%E9%9F%BF%E8%88%87%E6%B3%A8%E6%84%8F%E4%BA%8B%E9%A0%85-6195d10d4441
                 *
                 */
                Guid guid = Guid.NewGuid(); // 生成GUID token
                string token = guid.ToString("N");

                // 將token雜湊加密
                string tokenHash = BCrypt.Net.BCrypt.HashPassword(token);

                // 將token儲存到cookie中，並設置該cookie失效時間
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddMinutes(30); // 設定token的失效時間，作為忘記密碼連結失效時間
                option.HttpOnly = true; // 強制使用https存取cookie
                option.Secure = true; // 禁用js讀取cookie防止xss攻擊
                option.SameSite = SameSiteMode.None; // SameSiteMode設置為None以允許前端發送post請求存取cookie
                Response.Cookies.Append("forgetPwdToken", tokenHash, option);
                Response.Cookies.Append("InputEmail", request.Email.ToString(), option);

                return Ok(tokenHash);
            }
        }

        // 取得忘記密碼token的值
        [HttpPost("getforgetPwdtoken")]
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
            string? inputEmail = System.Net.WebUtility.UrlDecode(Request.Cookies["InputEmail"]); // 將URL編碼的字串轉換回原始的字串
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == inputEmail); 

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
