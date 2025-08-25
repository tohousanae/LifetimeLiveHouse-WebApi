using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using LifetimeLiveHouseContext = LifetimeLiveHouse.Access.Data.LifetimeLiveHouseContext;

namespace LifetimeLiveHouseWebAPI.Controllers.User
{
    [Authorize(Roles = "Member")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(LifetimeLiveHouseContext context) : ControllerBase
    {
        private readonly LifetimeLiveHouseContext _context = context;

        // 顯示個別會員資料
        [HttpGet("Member/{id}")]
        public async Task<ActionResult<MemberDTO>> GetUserInfo(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound("找不到此會員");
            }
            // 呼叫 ItemUser 方法並回傳結果
            return ItemUser(user);
        }

        // 登入api(cookie based驗證)
        [HttpPost("login")]

        public async Task<ActionResult<Member>> PostUserLogin(LoginDTO memberAccount)
        {
            //if (memberAccount == null || string.IsNullOrEmpty(memberAccount.Account) || string.IsNullOrEmpty(memberAccount.Password))
            //{
            //    ViewData["Error"] = "請輸入帳號和密碼";
            //    return View();
            //}

            //var user = await _context.MemberAccount.Include(u => u.Member)                                        //密碼必須先經過雜湊處理，再與資料庫中的密碼進行比對
            //    .FirstOrDefaultAsync(u => u.Account == memberAccount.Account && u.Password == ComputeSha256Hash(memberAccount.Password)); //12345678


            //if (user != null)
            //{
            //    var claims = new List<Claim>
            //        {
            //            new Claim(ClaimTypes.Actor, user.Account),
            //            new Claim(ClaimTypes.Role, "Member"),
            //             new Claim(ClaimTypes.Sid, user.MemberID),
            //              new Claim(ClaimTypes.Name, user.Member.Name)

            //        };

            //    var claimsIdentity = new ClaimsIdentity(claims, "MemberLogin");

            //    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            //    await HttpContext.SignInAsync("MemberLogin", claimsPrincipal); //把資料寫入 Cookie 進行登入狀態管理



            //    return RedirectToAction("Index", "Members"); // 登入成功後導向到 BooksManage 的 Index 頁面
            //}

            //ViewData["Error"] = "帳號或密碼錯誤，請重新輸入";


            //return View(memberAccount);

            // 檢查輸入的信箱是否為使用者輸入的信箱
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == memberAccount.Email);

            if (user == null)
            {
                return Unauthorized("電子郵件或密碼不存在");
            }

            // 檢查輸入的密碼是否為使用者輸入的密碼
            if (!BCrypt.Net.BCrypt.Verify(memberAccount.Password, user.Password))
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
                Response.Cookies.Append("UserId", BCrypt.Net.BCrypt.HashPassword(user.Id.ToString()), option);
                return Ok("登入成功");
            }
        }

        // 讀取cookie驗證是否登入
        [HttpPost("loginCkeck")]
        public IActionResult PostUserCheckLogin()
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
        public IActionResult PostUserLogout()
        {
            // 清除用户的身份验证凭证（例如，清除存储在 cookie 中的身份验证令牌）
            CookieOptions option = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true,
                Secure = true
            };
            Response.Cookies.Append("UserId", "", option);
            return Ok("登出成功");
            // 返回成功响应
        }

        // 查詢所有會員
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetUsers()
        {
            return await _context.User.ToListAsync();
        }

        // 查詢個別會員
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetUser(int id)
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
        public async Task<IActionResult> PutUser(int id, Member user)
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
        public async Task<ActionResult<Member>> PostUser(Member user)
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
        [HttpPost("headPictureUpload")]
        public string HeadUpload(IFormFile inputImage)
        {
            if (inputImage == null || inputImage.Length == 0)
            {
                return "沒有上傳檔案";
            }

            //只允許上傳圖片
            if (inputImage.ContentType != "image/jpeg" && inputImage.ContentType != "image/png" && inputImage.ContentType != "image/webp")
            {
                return "僅支援上傳.jpg、.png或webp格式"; ;
            }


            string fileName = Path.GetFileName(inputImage.FileName);
            string accessProjectPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Assets", "Images");
            Directory.CreateDirectory(accessProjectPath); // 若資料夾不存在則建立

            string filePath = Path.Combine(accessProjectPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                inputImage.CopyTo(stream);
            }

            return "檔案上傳成功!!";
        }

        private static MemberDTO ItemUser(Member u)
        {
            var returnUser = new MemberDTO
            {
                Name = u.Name,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Sex = u.Sex,
                Birthday = u.Birthday,
                MikuMikuPoint = u.MikuMikuPoint,
            };

            return returnUser;

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

//namespace HotelSystem.Areas.User.Controllers
//{
//    [Area("User")]
//    public class LoginController : Controller
//    {
//        private readonly HotelSysDBContext2 _context;

//        public LoginController(HotelSysDBContext2 context)
//        {

//            _context = context;
//        }
//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult Login()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Login(MemberAccount memberAccount)
//        {

//            if (memberAccount == null || string.IsNullOrEmpty(memberAccount.Account) || string.IsNullOrEmpty(memberAccount.Password))
//            {
//                ViewData["Error"] = "請輸入帳號和密碼";
//                return View();
//            }

//            var user = await _context.MemberAccount.Include(u => u.Member)                                        //密碼必須先經過雜湊處理，再與資料庫中的密碼進行比對
//                .FirstOrDefaultAsync(u => u.Account == memberAccount.Account && u.Password == ComputeSha256Hash(memberAccount.Password)); //12345678


//            if (user != null)
//            {
//                var claims = new List<Claim>
//                    {
//                        new Claim(ClaimTypes.Actor, user.Account),
//                        new Claim(ClaimTypes.Role, "Member"),
//                         new Claim(ClaimTypes.Sid, user.MemberID),
//                          new Claim(ClaimTypes.Name, user.Member.Name)

//                    };

//                var claimsIdentity = new ClaimsIdentity(claims, "MemberLogin");

//                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

//                await HttpContext.SignInAsync("MemberLogin", claimsPrincipal); //把資料寫入 Cookie 進行登入狀態管理



//                return RedirectToAction("Index", "Members"); // 登入成功後導向到 BooksManage 的 Index 頁面
//            }

//            ViewData["Error"] = "帳號或密碼錯誤，請重新輸入";


//            return View(memberAccount);

//        }

//        [Authorize]
//        public async Task<IActionResult> Logout()
//        {

//            await HttpContext.SignOutAsync("MemberLogin");// 清除登入狀態(清除Cookie的MemberLogin紀錄)
//            return RedirectToAction("Index", "Rooms"); // 登出後導向到 Login 頁面


//        }

//        // 新增一個靜態方法來進行 SHA256 雜湊
//        private static string ComputeSha256Hash(string rawData)
//        {
//            using (SHA256 sha256Hash = SHA256.Create())
//            {
//                // 計算雜湊值
//                byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawData));

//                // 將 byte 陣列轉換成十六進位字串
//                StringBuilder builder = new StringBuilder();
//                for (int i = 0; i < bytes.Length; i++)
//                {
//                    builder.Append(bytes[i].ToString("x2"));
//                }
//                return builder.ToString();
//            }
//        }
//    }
//}
