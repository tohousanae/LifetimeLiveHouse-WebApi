using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using LifetimeLiveHouseWebAPI.Modules.User.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

//using StackExchange.Redis;
var builder = WebApplication.CreateBuilder(args);

// 注入DBContext
builder.Services.AddDbContext<LifetimeLiveHouseSysDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("LifetimeLiveHouseSysDBConnection")));

builder.Services.AddDbContext<LifetimeLiveHouseSysDBContext2>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("LifetimeLiveHouseSysDBConnection")));


builder.Services.AddMailKit(config =>
{
    config.UseMailKit(new MailKitOptions()
    {
        Server = "smtp.gmail.com",
        Port = 587,
        SenderName = "Lifetime LiveHouse",
        SenderEmail = "saigyoujiyuyukoth@gmail.com",
        Account = "saigyoujiyuyukoth@gmail.com",
        Password = "sobi nwxj kusx wuss",
        Security = true
    });
});

// 設定選項
builder.Services.Configure<TwilioOptions>(builder.Configuration.GetSection("Twilio"));
builder.Services.AddSingleton(sp =>
{
    var opts = sp.GetRequiredService<IOptions<TwilioOptions>>().Value;
    Twilio.TwilioClient.Init(opts.AccountSid, opts.AuthToken);
    return Twilio.TwilioClient.GetRestClient();
});

// 住入服務
builder.Services.AddScoped<IForgetPasswordService, ForgetPasswordService>();
builder.Services.AddScoped<IMemberRegisterServices, MemberRegisterServices>();
builder.Services.AddScoped<IMemberLoginService, MemberLoginService>();

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

//
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
    opt.TokenLifespan = TimeSpan.FromHours(2));

//跨域存取政策
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", policy =>
    {
        //policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        policy.WithOrigins("http://localhost:5173").WithHeaders("*").WithMethods("*").AllowCredentials();
    });
});


// cookie驗證預設設定
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "MemberLogin";
})
    .AddCookie("MemberLogin", options =>
    {
        //options.LoginPath = "/api/auth/login";
        //options.LogoutPath = "/api/auth/logout";
        //以上兩條在web api當中沒用，因為web api不會重新導向
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.Cookie.HttpOnly = true; // 禁止 JavaScript 存取 Cookie防XSS攻擊。
        options.Cookie.SameSite = SameSiteMode.None; // 開放前端跨域存取cookie
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // 強制瀏覽器僅在 HTTPS 連線下傳送該 Cookie。
        options.SlidingExpiration = true; // 自動延長有效時間
    });
builder.Services.AddApplicationInsightsTelemetry();

////builder.Services
//    .AddIdentity<MemberAccount, IdentityRole>()
//    .AddEntityFrameworkStores<IdentityDbContext>()
//    .AddDefaultTokenProviders();

//// 註冊Token過期時間為2小時
//builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
//    opt.TokenLifespan = TimeSpan.FromHours(2));

var app = builder.Build();

//1.3.4 在Program.cs撰寫啟用Initializer的程式
//執行專案時自動載入初始資料
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;

    SeedData.Initialize(service);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // 加入swagger request duration顯示
        c.DisplayRequestDuration();
    });

}

app.UseHttpsRedirection();
app.MapSwagger().RequireAuthorization();

//實務上API並不會需要顯示靜態檔案，因為API通常是提供給前端使用的，前端會有自己的靜態檔案處理方式
//app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
