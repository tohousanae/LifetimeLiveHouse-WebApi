using LifetimeLiveHouse.Access.Data;
using LifetimeLiveHouseWebAPI.Services.Implementations;
using LifetimeLiveHouseWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

//using StackExchange.Redis;
var builder = WebApplication.CreateBuilder(args);

//// �I�s AddAuthorization �H�N�A�ȷs�W�ܬ̩ۨʴ��J (DI) �e��
//builder.Services.AddAuthorization();

// �`�JDBContext
builder.Services.AddDbContext<LifetimeLiveHouseSysDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("LifetimeLiveHouseSysDBConnection")));

builder.Services.AddDbContext<LifetimeLiveHouseSysDBContext2>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("LifetimeLiveHouseSysDBConnection")));

// ��J�A��
builder.Services.AddScoped<IForgetPasswordService, ForgetPasswordService>();
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
builder.Services.AddScoped<IMemberLoginService, MemberLoginService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// �[�J�����������O����֨��A��
builder.Services.AddDistributedMemoryCache();

// �[�Jredis�������֨��A��
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

//���s���F��
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", policy =>
    {
        //policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        policy.WithOrigins("http://localhost:5173").WithHeaders("*").WithMethods("*").AllowCredentials();
    });
});


// cookie���ҹw�]�]�w
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "MemberLogin";
})
    .AddCookie("MemberLogin", options =>
    {
        //options.LoginPath = "/api/auth/login";
        //options.LogoutPath = "/api/auth/logout";
        //�H�W����bweb api���S�ΡA�]��web api���|���s�ɦV
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.Cookie.HttpOnly = true; // �T�� JavaScript �s�� Cookie��XSS�����C
        options.Cookie.SameSite = SameSiteMode.None; // �}��e�ݸ��s��cookie
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // �j���s�����Ȧb HTTPS �s�u�U�ǰe�� Cookie�C
        options.SlidingExpiration = true; // �۰ʩ������Įɶ�
    });

////builder.Services
//    .AddIdentity<MemberAccount, IdentityRole>()
//    .AddEntityFrameworkStores<IdentityDbContext>()
//    .AddDefaultTokenProviders();

//// ���UToken�L���ɶ���2�p��
//builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
//    opt.TokenLifespan = TimeSpan.FromHours(2));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // �[�Jswagger request duration���
        c.DisplayRequestDuration();
    });

}

app.UseHttpsRedirection();
app.MapSwagger().RequireAuthorization();

//��ȤWAPI�ä��|�ݭn����R�A�ɮסA�]��API�q�`�O���ѵ��e�ݨϥΪ��A�e�ݷ|���ۤv���R�A�ɮ׳B�z�覡
//app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
