using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.DTOs.Users;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifetimeLiveHouseWebAPI.Modules.User.Services
{
    public class MemberProfileService(LifetimeLiveHouseSysDBContext context) : IMemberProfileService
    {
        private readonly LifetimeLiveHouseSysDBContext _context = context;

        // ==========================================
        // 📖 取得會員資料
        // ==========================================
        public async Task<ActionResult<object>> GetMemberProfileAsync(long memberId)
        {
            var profile = await _context.Member
                .AsNoTracking()
                .Where(m => m.MemberID == memberId)
                .Select(m => new
                {
                    m.MemberID,
                    m.Name,
                    m.Birthday,
                    m.CellphoneNumber,
                    m.Cash,
                    m.MemberPoint,
                    m.CreatedDate,
                    m.StatusCode,

                    // 從 MemberAccount 撈取信箱
                    Email = _context.MemberAccount
                                .Where(ma => ma.MemberID == m.MemberID)
                                .Select(ma => ma.Email)
                                .FirstOrDefault(),

                    // 驗證狀態
                    IsEmailVerified = m.MemberEmailVerificationStatus != null && m.MemberEmailVerificationStatus.IsEmailVerified,
                    IsPhoneVerified = m.MemberPhoneVerificationStatus != null && m.MemberPhoneVerificationStatus.IsPhoneVerified
                })
                .FirstOrDefaultAsync();

            if (profile == null)
                return new NotFoundObjectResult("找不到該會員資料");

            return new OkObjectResult(profile);
        }

        // ==========================================
        // ✍️ 更新會員資料 (支援修改信箱主鍵與狀態重置)
        // ==========================================
        public async Task<ActionResult<object>> UpdateMemberProfileAsync(long memberId, MemberUpdateDTO dto)
        {
            // 1. 取得目前的信箱
            var currentEmail = await _context.MemberAccount
                .Where(a => a.MemberID == memberId)
                .Select(a => a.Email)
                .FirstOrDefaultAsync();

            if (currentEmail == null)
                return new BadRequestObjectResult("找不到該會員資料");

            bool isEmailChanged = false;

            // 💡 啟用資料庫交易 (Transaction)：確保信箱修改、狀態重置、會員基本資料更新這三個動作
            // 要嘛全部成功，要嘛全部失敗退回，絕對不會發生資料不一致的問題。
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 2. 判斷信箱是否有變動
                if (!string.Equals(currentEmail, dto.Email, StringComparison.OrdinalIgnoreCase))
                {
                    // 檢查新信箱是否已被其他人註冊
                    if (await _context.MemberAccount.AnyAsync(a => a.Email == dto.Email))
                        return new BadRequestObjectResult("此信箱已被其他帳號使用");

                    isEmailChanged = true;

                    // 🚀 核心修復：因為 Email 是 MemberAccount 的 PK，不能用傳統追蹤修改。
                    // 這裡直接對資料庫下達 UPDATE 語法強制修改。
                    await _context.MemberAccount
                        .Where(a => a.MemberID == memberId)
                        .ExecuteUpdateAsync(s => s.SetProperty(p => p.Email, dto.Email));

                    // 🔄 重置信箱驗證狀態與 Token
                    var statusRows = await _context.MemberEmailVerificationStatus
                        .Where(s => s.MemberID == memberId)
                        .ExecuteUpdateAsync(s => s
                            .SetProperty(p => p.IsEmailVerified, false)
                            .SetProperty(p => p.EmailVerificationTokenHash, (string?)null)
                            .SetProperty(p => p.EmailVerificationTokenExpiry, (DateTime?)null));

                    // 防呆：如果資料庫裡剛好沒有該會員的驗證狀態紀錄，手動補上一筆
                    if (statusRows == 0)
                    {
                        _context.MemberEmailVerificationStatus.Add(new MemberEmailVerificationStatus
                        {
                            MemberID = memberId,
                            IsEmailVerified = false
                        });
                        await _context.SaveChangesAsync();
                    }
                }

                // 3. 更新 Member 資料表的基本資料
                await _context.Member
                    .Where(m => m.MemberID == memberId)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(p => p.Name, dto.Name)
                        .SetProperty(p => p.CellphoneNumber, dto.CellphoneNumber)
                        .SetProperty(p => p.Birthday, dto.Birthday));

                // 4. 全部修改成功，提交交易！
                await transaction.CommitAsync();

                return new OkObjectResult(new
                {
                    message = isEmailChanged ? "資料更新成功！信箱已變更，請重新驗證信箱。" : "會員資料更新成功！",
                    isEmailChanged
                });
            }
            catch (Exception)
            {
                // 如果過程中發生任何錯誤 (例如斷線或資料庫鎖死)，整組動作取消復原
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}