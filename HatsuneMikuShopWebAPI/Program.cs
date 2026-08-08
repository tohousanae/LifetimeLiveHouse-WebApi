using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using LifetimeLiveHouseWebAPI.Modules.User.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("LifetimeLiveHouseSysDBConnection");

// 注入 DBContext 並套用剛剛決定的連線字串
builder.Services.AddDbContext<LifetimeLiveHouseSysDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<LifetimeLiveHouseSysDBContext2>(options =>
    options.UseSqlServer(connectionString));

// 改寫 MailKit 註冊邏輯
builder.Services.AddMailKit(config =>
{
    config.UseMailKit(new MailKitOptions()
    {
        Server = builder.Configuration["MailKit:Server"],
        Port = int.Parse(builder.Configuration["MailKit:Port"] ?? "587"),
        SenderName = builder.Configuration["MailKit:SenderName"],
        SenderEmail = builder.Configuration["MailKit:SenderEmail"],
        Account = builder.Configuration["MailKit:Account"],
        // 關鍵：密碼從組態(環境變數)動態載入
        Password = builder.Configuration["MailKit:Password"],
        Security = bool.Parse(builder.Configuration["MailKit:Security"] ?? "true")
    });
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
        policy.WithOrigins("https://livetimelivehouse.sakuyaonline.uk")
              .AllowAnyHeader()   // 改用這個
              .AllowAnyMethod()   // 改用這個
              .AllowCredentials();
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

// 👉 將 Azure 監控設定移到這裡 (必須在 Build 之前)
// 只有在非開發環境 (例如 Production) 才啟用 Application Insights
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddApplicationInsightsTelemetry();
}

////builder.Services
//    .AddIdentity<MemberAccount, IdentityRole>()
//    .AddEntityFrameworkStores<IdentityDbContext>()
//    .AddDefaultTokenProviders();

//// 註冊Token過期時間為2小時
//builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
//    opt.TokenLifespan = TimeSpan.FromHours(2));

// 註冊自訂的 OAuth2 寄信服務
builder.Services.AddScoped<EmailService>();

// twilio設定綁定
builder.Services.Configure<TwilioOptions>(builder.Configuration.GetSection("Twilio"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<TwilioOptions>>().Value);

var app = builder.Build();

// --------------- 在 Build 之後初始化 Twilio ---------------
var twilioOpts = app.Services.GetRequiredService<TwilioOptions>();

// 在開發模式使用秘密管理員設定
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
    // 偵錯用：印出是否有設定（切勿印出完整 AuthToken 到生產 log）
    Console.WriteLine($"[DEBUG] Twilio AccountSid set? {!string.IsNullOrWhiteSpace(twilioOpts.AccountSid)}");
    Console.WriteLine($"[DEBUG] Twilio AuthToken set? {!string.IsNullOrWhiteSpace(twilioOpts.AuthToken)}");
    Console.WriteLine($"[DEBUG] Twilio VerifyServiceSid set? {!string.IsNullOrWhiteSpace(twilioOpts.VerifyServiceSid)}");

    if (string.IsNullOrWhiteSpace(twilioOpts.AccountSid) || string.IsNullOrWhiteSpace(twilioOpts.AuthToken))
    {
        // 開發階段可以直接丟例外，提醒缺少設定
        throw new InvalidOperationException("Twilio AccountSid 或 AuthToken 未設定。請檢查 appsettings / user-secrets / environment variables。");
    }
}

// 呼叫初始化（這會設定 TwilioClient 的全域認證）
Twilio.TwilioClient.Init(twilioOpts.AccountSid, twilioOpts.AuthToken);
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
// 【架構設定】外部 HTTPS 加密已交由 Cloudflare Tunnel 處理 (SSL Offloading)
// 本機端僅需專注監聽 HTTP 流量，故停用 HTTPS 重新導向以避免無窮迴圈 (Infinite Redirect Loop)
//app.UseHttpsRedirection();
app.MapSwagger().RequireAuthorization();

//實務上API並不會需要顯示靜態檔案，因為API通常是提供給前端使用的，前端會有自己的靜態檔案處理方式
//app.UseStaticFiles();

app.UseCors("MyCorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();