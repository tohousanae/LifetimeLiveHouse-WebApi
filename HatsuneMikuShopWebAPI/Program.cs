using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using LifetimeLiveHouseContext = LifetimeLiveHouse.Access.Data.LifetimeLiveHouseContext;
var builder = WebApplication.CreateBuilder(args);

// �`�JMikuMusicShopContext�����O
builder.Services.AddDbContext<LifetimeLiveHouseContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("LifetimeLiveHouseSysDBConnection")));

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

// CORS��ӷ��@�γ]�w
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:5173").WithHeaders("*").WithMethods("*").AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapSwagger().RequireAuthorization();

//��ȤWAPI�ä��|�ݭn����R�A�ɮסA�]��API�q�`�O���ѵ��e�ݨϥΪ��A�e�ݷ|���ۤv���R�A�ɮ׳B�z�覡
//app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
