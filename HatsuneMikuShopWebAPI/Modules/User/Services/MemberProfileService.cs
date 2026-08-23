//using LifetimeLiveHouse.Access.Data;
//using LifetimeLiveHouseWebAPI.DTOs.Users;
//using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace LifetimeLiveHouseWebAPI.Modules.User.Services
//{
//    public class MemberProfileService(LifetimeLiveHouseSysDBContext context) : IMemberProfileService
//    {
//        private readonly LifetimeLiveHouseSysDBContext _context = context;

//        // ==========================================
//        // 📖 取得會員資料 (完全對齊資料字典)
//        // ==========================================
//        public async Task<ActionResult<object>> GetMemberProfileAsync(long memberId)
//        {
//            var profile = await _context.Member
//                .AsNoTracking() // 唯讀查詢不寫入記憶體快取，降低 Server 負擔
//                .Where(m => m.MemberID == memberId)
//                .Select(m => new
//                {
//                    // 基礎個人資料
//                    m.MemberID,
//                    m.Name,
//                    m.Birthday,
//                    m.CellphoneNumber,

//                    // 💰 平台專屬數值與狀態 (依據資料字典)
//                    m.Cash,             // 儲值金
//                    m.MemberPoint,      // 回饋點數
//                    m.CreatedDate,      // 創建日期
//                    m.StatusCode,       // 狀態編號 (0: 正常)

//                    // 💡 核心修正：利用 _context 直接從 MemberAccount 資料表撈取 Email
//                    // 這樣就不需要依賴 Member 模型中是否有 MemberAccount 的導覽屬性了
//                    Email = _context.MemberAccount
//                                .Where(ma => ma.MemberID == m.MemberID)
//                                .Select(ma => ma.Email)
//                                .FirstOrDefault(),

//                    // 🔗 驗證狀態
//                    IsEmailVerified = m.MemberEmailVerificationStatus != null && m.MemberEmailVerificationStatus.IsEmailVerified,
//                    IsPhoneVerified = m.MemberPhoneVerificationStatus != null && m.MemberPhoneVerificationStatus.IsPhoneVerified
//                })
//                .FirstOrDefaultAsync();

//            if (profile == null)
//                return new NotFoundObjectResult("找不到該會員資料");

//            return new OkObjectResult(profile);
//        }

//        // ==========================================
//        // ✍️ 更新會員資料 (完全對齊資料字典)
//        // ==========================================
//        public async Task<ActionResult<string>> UpdateMemberProfileAsync(long memberId, MemberUpdateDTO dto)
//        {
//            // 💡 效能優化：使用 ExecuteUpdateAsync 直接在 DB 端更新
//            // 注意：這裡只更新 Name, CellphoneNumber, Birthday。完全捨棄了資料字典中不存在的 Sex 欄位
//            var rows = await _context.Member
//                .Where(m => m.MemberID == memberId)
//                .ExecuteUpdateAsync(s => s
//                    .SetProperty(p => p.Name, dto.Name)
//                    .SetProperty(p => p.CellphoneNumber, dto.CellphoneNumber)
//                    .SetProperty(p => p.Birthday, dto.Birthday));

//            if (rows == 0)
//                return new BadRequestObjectResult("更新失敗，會員不存在或資料無異動");

//            return new OkObjectResult("會員資料更新成功");
//        }
//    }
//}