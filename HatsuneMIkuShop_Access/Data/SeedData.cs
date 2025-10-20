using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LifetimeLiveHouse.Access.Data
{
    public class SeedData
    {
        //1.3.3 撰寫SeedData類別的內容
        //      (1)撰寫靜態方法 Initialize(IServiceProvider serviceProvider)
        //      (2)撰寫Book及ReBook資料表內的初始資料程式
        //      (3)撰寫上傳圖片的程式
        //      (4)加上 using() 及 判斷資料庫是否有資料的程式


        //(1)撰寫靜態方法 Initialize(IServiceProvider serviceProvider)
        public static void Initialize(IServiceProvider serviceProvider)
        {



            //(4)加上 using () 及 判斷資料庫是否有資料的程式
            using (LifetimeLiveHouseSysDBContext context = new LifetimeLiveHouseSysDBContext(serviceProvider.GetRequiredService<DbContextOptions<LifetimeLiveHouseSysDBContext>>()))
            {

                //(4)加上 using () 及 判斷資料庫是否有資料的程式
                if (!context.Member.Any())
                {

                    context.Member.AddRange(
                        new Member
                        {
                            Name = "林小明",
                            Birthday = DateTime.Parse("1900-01-01"),
                            CellphoneNumber = "0912345678",
                            StatusCode = "0",
                            Cash = 1000,
                            MemberPoint = 100
                        },
                        new Member
                        {
                            Name = "張麗麗",
                            Birthday = DateTime.Parse("1985-05-05"),
                            CellphoneNumber = "0987654321",
                            StatusCode = "0",
                            Cash = 2000,
                            MemberPoint = 200
                        }
                    );

                    context.SaveChanges();


                    context.MemberAccount.AddRange(

                        new MemberAccount
                        {
                            MemberID = context.Member.First(m => m.Name == "林小明").MemberID,
                        },
                        new ReBook
                        {
                            ReBookID = Guid.NewGuid().ToString(),
                            Description = "我不喜歡....",
                            Author = "柯南",
                            CreatedDate = DateTime.Now,
                            BookID = guid[0]
                        },
                        new ReBook
                        {
                            ReBookID = Guid.NewGuid().ToString(),
                            Description = "你最好餓死",
                            Author = "小蘭",
                            CreatedDate = DateTime.Now,
                            BookID = guid[0]
                        },
                        new ReBook
                        {
                            ReBookID = Guid.NewGuid().ToString(),
                            Description = "高麗菜這樣超好吃啊～",
                            Author = "小英",
                            CreatedDate = DateTime.Now,
                            BookID = guid[1]
                        },
                        new ReBook
                        {
                            ReBookID = Guid.NewGuid().ToString(),
                            Description = "口味似乎偏辣",
                            Author = "阿狗",
                            CreatedDate = DateTime.Now,
                            BookID = guid[2]
                        },
                        new ReBook
                        {
                            ReBookID = Guid.NewGuid().ToString(),
                            Description = "我還是喜歡生魚片的握壽司",
                            Author = "嫩嫩",
                            CreatedDate = DateTime.Now,
                            BookID = guid[3]
                        },
                        new ReBook
                        {
                            ReBookID = Guid.NewGuid().ToString(),
                            Description = "我也是喜歡生魚片的握壽司，但這個也不錯",
                            Author = "王小花",
                            CreatedDate = DateTime.Now,
                            BookID = guid[3]
                        },
                        new ReBook
                        {
                            ReBookID = Guid.NewGuid().ToString(),
                            Description = "三杯雞比較對味",
                            Author = "芷若",
                            CreatedDate = DateTime.Now,
                            BookID = guid[4]
                        }

                        );
                    context.SaveChanges();
                }

                if (!context.Member.Any())
                {

                    context.Member.AddRange(
                        new Member
                        {
                            Name = "林小明",
                            Birthday = DateTime.Parse("1900-01-01"),
                            CellphoneNumber = "0912345678",
                            StatusCode = "0",
                            Cash = 1000,
                            MemberPoint = 100
                        },
                        new Member
                        {
                            Name = "張麗麗",
                            Birthday = DateTime.Parse("1985-05-05"),
                            CellphoneNumber = "0987654321",
                            StatusCode = "0",
                            Cash = 2000,
                            MemberPoint = 200
                        }
                    );

                    context.SaveChanges();
                }
            } //using結束
        }

    }
}
