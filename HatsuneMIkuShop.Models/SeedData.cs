﻿//using LifetimeLiveHouse.Access;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//// 請確認 LifetimeLiveHouseSysDBContext 類別已正確定義於 LifetimeLiveHouse.Models 命名空間
//// 若 LifetimeLiveHouseSysDBContext 定義於其他命名空間，請補上 using 指示詞
//// 例如：using LifetimeLiveHouseSysDBContextNamespace;

//namespace LifetimeLiveHouse.Models
//{
//    public class SeedData
//    {
//        public static void Initialize(IServiceProvider serviceProvider)
//        {
//            // 請確認 LifetimeLiveHouseSysDBContext 已正確定義
//            using (LifetimeLiveHouseSysDBContext context = new LifetimeLiveHouseSysDBContext(serviceProvider.GetRequiredService<DbContextOptions<LifetimeLiveHouseSysDBContext>>()))
//            {
//                if (!context.Book.Any())
//                {
//                    string[] guid = { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };

//                    context.Book.AddRange(
//                        new Book
//                        {
//                            BookID = guid[0],
//                            Title = "櫻桃鴨",
//                            Description = "這看起來好好吃哦!!!",
//                            Author = "Jack",
//                            Photo = guid[0] + ".jpg",
//                            CreatedDate = DateTime.Now
//                        },
//                        new Book
//                        {
//                            BookID = guid[1],
//                            Title = "鴨油高麗菜",
//                            Description = "好像稍微有點油....",
//                            Author = "Mary",
//                            Photo = guid[1] + ".jpg",
//                            CreatedDate = DateTime.Now
//                        },
//                        new Book
//                        {
//                            BookID = guid[2],
//                            Title = "鴨油麻婆豆腐",
//                            Description = "這太下飯了！可以吃好幾碗白飯",
//                            Photo = guid[2] + ".jpg",
//                            Author = "王小花",
//                            CreatedDate = DateTime.Now
//                        },
//                        new Book
//                        {
//                            BookID = guid[3],
//                            Title = "櫻桃鴨握壽司",
//                            Description = "握壽司就是好吃！",
//                            Photo = guid[3] + ".jpg",
//                            Author = "王小花",
//                            CreatedDate = DateTime.Now
//                        },
//                        new Book
//                        {
//                            BookID = guid[4],
//                            Title = "三杯鴨",
//                            Description = "鴨肉鮮甜",
//                            Photo = guid[4] + ".jpg",
//                            Author = "Jack",
//                            CreatedDate = DateTime.Now
//                        }
//                    );

//                    context.SaveChanges();

//                    context.ReBook.AddRange(
//                        new ReBook
//                        {
//                            ReBookID = Guid.NewGuid().ToString(),
//                            Description = "我也覺得好吃！",
//                            Author = "小蘭",
//                            CreatedDate = DateTime.Now,
//                            BookID = guid[0]
//                        },
//                        new ReBook
//                        {
//                            ReBookID = Guid.NewGuid().ToString(),
//                            Description = "我不喜歡....",
//                            Author = "柯南",
//                            CreatedDate = DateTime.Now,
//                            BookID = guid[0]
//                        },
//                        new ReBook
//                        {
//                            ReBookID = Guid.NewGuid().ToString(),
//                            Description = "你最好餓死",
//                            Author = "小蘭",
//                            CreatedDate = DateTime.Now,
//                            BookID = guid[0]
//                        },
//                        new ReBook
//                        {
//                            ReBookID = Guid.NewGuid().ToString(),
//                            Description = "高麗菜這樣超好吃啊～",
//                            Author = "小英",
//                            CreatedDate = DateTime.Now,
//                            BookID = guid[1]
//                        },
//                        new ReBook
//                        {
//                            ReBookID = Guid.NewGuid().ToString(),
//                            Description = "口味似乎偏辣",
//                            Author = "阿狗",
//                            CreatedDate = DateTime.Now,
//                            BookID = guid[2]
//                        },
//                        new ReBook
//                        {
//                            ReBookID = Guid.NewGuid().ToString(),
//                            Description = "我還是喜歡生魚片的握壽司",
//                            Author = "嫩嫩",
//                            CreatedDate = DateTime.Now,
//                            BookID = guid[3]
//                        },
//                        new ReBook
//                        {
//                            ReBookID = Guid.NewGuid().ToString(),
//                            Description = "我也是喜歡生魚片的握壽司，但這個也不錯",
//                            Author = "王小花",
//                            CreatedDate = DateTime.Now,
//                            BookID = guid[3]
//                        },
//                        new ReBook
//                        {
//                            ReBookID = Guid.NewGuid().ToString(),
//                            Description = "三杯雞比較對味",
//                            Author = "芷若",
//                            CreatedDate = DateTime.Now,
//                            BookID = guid[4]
//                        }
//                    );
//                    context.SaveChanges();

//                    string SeedPhotosPath = Path.Combine(Directory.GetCurrentDirectory(), "SeedPhotos");
//                    string BookPhotosPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BookPhotos");

//                    string[] files = Directory.GetFiles(SeedPhotosPath);

//                    for (int i = 0; i < files.Length; i++)
//                    {
//                        string destFile = Path.Combine(BookPhotosPath, guid[i] + ".jpg");
//                        File.Copy(files[i], destFile);
//                    }
//                }
//            }
//        }
//    }
//}
