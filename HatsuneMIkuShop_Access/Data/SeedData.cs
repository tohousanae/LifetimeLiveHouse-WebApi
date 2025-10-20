using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        // 將預設資料寫成獨立方法並且以同步方式決定載入資料順序
        var options = serviceProvider.GetRequiredService<DbContextOptions<LifetimeLiveHouseSysDBContext>>();
        using var context = new LifetimeLiveHouseSysDBContext(options);

        SeedMemberStatus(context);
        SeedMembersAndAccounts(context);
    }

    private static void SeedMemberStatus(LifetimeLiveHouseSysDBContext context)
    {
        if (context.MemberStatus.Any())
            return;

        context.MemberStatus.AddRange(
            new MemberStatus { StatusCode = "0", Status = "正常" },
            new MemberStatus { StatusCode = "1", Status = "停權中" },
            new MemberStatus { StatusCode = "2", Status = "警告中" }
        );
        context.SaveChanges();
    }

    private static void SeedMembersAndAccounts(LifetimeLiveHouseSysDBContext context)
    {
        if (context.Member.Any())
            return;

        // 新增會員
        var member1 = new Member
        {
            Name = "林小明",
            Birthday = DateTime.Parse("1900-01-01"),
            CellphoneNumber = "0912345678",
            StatusCode = "0",
            Cash = 1000,
            MemberPoint = 100
        };
        var member2 = new Member
        {
            Name = "張麗麗",
            Birthday = DateTime.Parse("1985-05-05"),
            CellphoneNumber = "0987654321",
            StatusCode = "0",
            Cash = 2000,
            MemberPoint = 200
        };
        context.Member.AddRange(member1, member2);
        context.SaveChanges();

        // 新增會員帳號
        context.MemberAccount.AddRange(
            new MemberAccount
            {
                MemberID = member1.MemberID,
                Email = "support@sakuyaonline.uk",
                Password = BCrypt.Net.BCrypt.HashPassword("#Aa123456")
            },
            new MemberAccount
            {
                MemberID = member2.MemberID,
                Email = "yukaribba17@yahoo.com",  // 修正 email 錯字
                Password = BCrypt.Net.BCrypt.HashPassword("%Bb789012")
            }
        );
        context.SaveChanges();

        // 新增驗證狀態
        context.MemberVerificationStatus.AddRange(
            new MemberVerificationStatus
            {
                MemberID = member1.MemberID,
                PhoneVerificationStatus = false,
                EmailVerificationStatus = false
            },
            new MemberVerificationStatus
            {
                MemberID = member2.MemberID,
                PhoneVerificationStatus = false,
                EmailVerificationStatus = false
            }
        );
        context.SaveChanges();
    }
}
