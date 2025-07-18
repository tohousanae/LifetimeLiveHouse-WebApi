<<<<<<<< HEAD:HatsuneMikuShopWebAPI/Program.cs
using HatsuneMikuShopWebAPI.Models;
========
using HatsuneMikuShopAPI.Models;
>>>>>>>> abfe7156f0424137ca89cb0a14e9dcaa344ae50c:HatsuneMikuShopAPI/HatsuneMikuShopAPI/Program.cs
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
var builder = WebApplication.CreateBuilder(args);

// 注入MikuMusicShopContext的類別
builder.Services.AddDbContext<MikuMusicShopContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("MikuMusicShopConnection")));

// Add services to the container.
<<<<<<<< HEAD:HatsuneMikuShopWebAPI/Program.cs

========
<<<<<<<< Updated upstream:HatsuneMikuShopAPI/HatsuneMikuShopAPI/Program.cs

========
>>>>>>>> Stashed changes:HatsuneMikuShopAPI/Program.cs
>>>>>>>> abfe7156f0424137ca89cb0a14e9dcaa344ae50c:HatsuneMikuShopAPI/HatsuneMikuShopAPI/Program.cs
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 加入本機分散式記憶體快取服務
builder.Services.AddDistributedMemoryCache();

// 加入redis分散式快取服務
//builder.Services.AddSingleton<IConnectionMultiplexer>(
//    ConnectionMultiplexer.Connect(
//        new ConfigurationOptions()
//        {
//            EndPoints = { { "localhost", 6379 } }
//        }
//    )
// );

// CORS跨來源共用設定
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:5173").WithHeaders("*").WithMethods("*").AllowCredentials();
    });
});
<<<<<<<< HEAD:HatsuneMikuShopWebAPI/Program.cs


========
>>>>>>>> abfe7156f0424137ca89cb0a14e9dcaa344ae50c:HatsuneMikuShopAPI/HatsuneMikuShopAPI/Program.cs
var app = builder.Build();

<<<<<<<< Updated upstream:HatsuneMikuShopAPI/HatsuneMikuShopAPI/Program.cs
// Configure the HTTP request pipeline.
<<<<<<<< HEAD:HatsuneMikuShopWebAPI/Program.cs
========
========
>>>>>>>> Stashed changes:HatsuneMikuShopAPI/Program.cs
>>>>>>>> abfe7156f0424137ca89cb0a14e9dcaa344ae50c:HatsuneMikuShopAPI/HatsuneMikuShopAPI/Program.cs
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapSwagger().RequireAuthorization();

<<<<<<<< HEAD:HatsuneMikuShopWebAPI/Program.cs
//實務上並不會使用靜態檔案，因為API通常是提供給前端使用的，前端會有自己的靜態檔案處理方式
//app.UseStaticFiles();

========
>>>>>>>> abfe7156f0424137ca89cb0a14e9dcaa344ae50c:HatsuneMikuShopAPI/HatsuneMikuShopAPI/Program.cs
app.UseAuthorization();

app.MapControllers();

app.Run();
