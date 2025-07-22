using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HatsuneMikuShopWebAPI.DTO.Users;
using HatsuneMIkuShop.Models;
using MikuMusicShopContext = HatsuneMIkuShop.Access.Data.MikuMusicShopContext;

namespace HatsuneMikuShopWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MikuMusicShopContext _context;

        public UserController(MikuMusicShopContext context)
        {
            _context = context;
        }

        // 會員中心，顯示會員資料
        [HttpGet("user/{id}")]
        public async Task<ActionResult<UserDto>> GetUserInfo(int inputId)
        {
            /// <summary>
            /// 顯示單個會員資料
            /// </summary>
            /// <param name="inputId">輸入會員Id</param>
            /// <returns>取得單個會員的資料</returns>
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
                Birthday = user.Birthday,
                MikuMikuPoint = user.MikuMikuPoint,
            };
            return userManageDto;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDTO inputRegisterInfo)
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
            // 檢查註冊的信箱或電話是否為黑名單
            /*
            else if {
                return Unauthorized("信箱為違規帳號信箱，無法註冊");
                return Unauthorized("手機號碼為違規帳號手機號碼，無法註冊");
            } 
             */
            else
            {
                
                // 將 RegisterDTO 轉換為 User 模型
                User user = new()
                {
                    Name = inputRegisterInfo.Name,
                    Email = inputRegisterInfo.Email,
                    PhoneNumber = inputRegisterInfo.PhoneNumber,
                    Password = BCrypt.Net.BCrypt.HashPassword(inputRegisterInfo.Password), // 密碼哈希加密與加鹽處理
                    Sex = inputRegisterInfo.Sex,
                    Birthday = inputRegisterInfo.Birthday,
                };

                _context.User.Add(user);
                await _context.SaveChangesAsync();
                return Ok("註冊成功");
            }
        }

        // 登入api(cookie based驗證)
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginDTO inputLoginInfo)
        {
            // 檢查輸入的信箱是否為使用者輸入的信箱
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == inputLoginInfo.Email); 

            if (user == null)
            {
                return Unauthorized("電子郵件或密碼不存在"); 
            }

            // 檢查輸入的密碼是否為使用者輸入的密碼
            if (!BCrypt.Net.BCrypt.Verify(inputLoginInfo.Password, user.Password))
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

        // 上傳頭像
        [HttpPost("headPictureUpload")] //紀錄會員id、頭像檔名、頭像圖片檔案，頭像更新時刪除原有檔案
        public string HeadUpload(IFormFile inputImage)
        {
            if (inputImage == null || inputImage.Length == 0)
            {
                return "沒有上傳檔案";
            }

            //只允許上傳圖片
            if (inputImage.ContentType != "image/jpeg" && inputImage.ContentType != "image/png")
            {
                return "僅支援上傳.jpg或.png格式"; ;
            }


            //取得檔案名稱
            string fileName = Path.GetFileName(inputImage.FileName);

            //取得檔案的完整路徑
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "images", fileName);
            // /wwwroot/Photos/xxx.jpg

            //將檔案上傳並儲存於指定的路徑

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                inputImage.CopyTo(fs);
            }

            return "檔案上傳成功!!";
        }
    }
}
