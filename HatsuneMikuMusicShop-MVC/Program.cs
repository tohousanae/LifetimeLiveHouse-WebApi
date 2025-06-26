using HatsuneMikuMusicShop_MVC.Models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
var builder = WebApplication.CreateBuilder(args);

// 注入MikuMusicShopContext的類別
builder.Services.AddDbContext<MikuMusicShopContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("MikuMusicShopConnection")));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 加入本機分散式記憶體快取服務
builder.Services.AddDistributedMemoryCache();

// 加入redis分散式快取服務
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(
        new ConfigurationOptions()
        {
            EndPoints = { { "localhost", 6379 } }
        }
    )
 );

// redis win11安裝教學
//https://redis.io/blog/install-redis-windows-11/

// CORS跨來源共用設定
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:5173").WithHeaders("*").WithMethods("*").AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
