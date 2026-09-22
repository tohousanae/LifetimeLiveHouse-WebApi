using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouse.Models.CustomModel;
using LifetimeLiveHouseWebAPI.Modules.User.Interfaces;
using LifetimeLiveHouseWebAPI.Modules.User.Services;
using LifetimeLiveHouseWebAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Antiforgery; // 👇 新增：引入 Antiforgery 命名空間

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("LifetimeLiveHouseSysDBConnection");

// 注入 DBContext 並套用剛剛決定的連線字串
builder.Services.AddDbContext<LifetimeLiveHouseSysDBContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddDbContext<LifetimeLiveHouseSysDBContext2>(options =>
//    options.UseSqlServer(connectionString));

// 住入服務
builder.Services.AddScoped<IForgetPasswordService, ForgetPasswordService>();
builder.Services.AddScoped<IMemberLoginService, MemberLoginService>();
builder.Services.AddScoped<IMemberProfileService, MemberProfileService>();
builder.Services.AddScoped<IMemberRegisterService, MemberRegisterService>();
builder.Services.AddScoped<IMemberVerificationService, MemberVerificationService>();
// 註冊 Token 清理背景排程
builder.Services.AddHostedService<TokenCleanupBackgroundService>();

builder.Services.AddControllers();

// 加入 CSRF 防禦服務，設定前端回傳時使用的 Header 名稱
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    // 預設發送給前端的 Cookie 名稱為 XSRF-TOKEN，Vue Axios 預設會去抓這個名字
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 註冊 Redis 分散式快取
builder.Services.AddStackExchangeRedisCache(options =>
{
    // 💡 絕對不要在程式碼裡寫死預設值，一律交給環境變數或組態檔決定
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "LifetimeLiveHouse_";
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
    opt.TokenLifespan = TimeSpan.FromHours(2));

// 1. 讀取前端網址變數 (若沒設定，預設給正式站網址作為保底)
var frontendBaseUrl = builder.Configuration["FrontendBaseUrl"] ?? "https://livetimelivehouse.sakuyaonline.uk";

// 2. 跨域存取政策
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", policy =>
    {
        // 3. 注入環境變數網址
        policy.WithOrigins(frontendBaseUrl)
              .AllowAnyHeader()
              .AllowAnyMethod()
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
        options.Cookie.SameSite = SameSiteMode.None; // 開放前端跨域存取cookie，前後端分離部署架構(如前端為vue3)需要另外實作防CSRF防偽權杖，不然前後端分離網站會無法串接API
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // 強制瀏覽器僅在 HTTPS 連線下傳送該 Cookie。
        options.SlidingExpiration = true; // 自動延長有效時間
    });

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

// 💡 必須在 UseAuthorization 之前加上這行，Cookie 驗證才會生效！
app.UseAuthentication();
app.UseAuthorization();

// 👇 新增第二段：加入自訂 Middleware，發送 CSRF Token 到前端的 Cookie 中
// 必須放在 UseCors 與 UseAuthorization 之後，MapControllers 之前
app.Use(next => context =>
{
    var antiforgery = app.Services.GetRequiredService<IAntiforgery>();
    var tokens = antiforgery.GetAndStoreTokens(context);

    if (tokens.RequestToken != null)
    {
        // 將 Token 寫入前端可讀取的 Cookie (注意：不能設為 HttpOnly，且跨域需設為 SameSite=None)
        context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
            new CookieOptions()
            {
                HttpOnly = false, // 必須為 false，讓 Vue / Axios 可以用 JavaScript 讀取到
                SameSite = SameSiteMode.None,
                Secure = app.Environment.IsDevelopment() ? false : true // 配合 HTTP/HTTPS 動態切換
            });
    }

    return next(context);
});

app.MapControllers();

app.Run();