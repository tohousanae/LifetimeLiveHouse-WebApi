<<<<<<<< HEAD:HatsuneMikuShopWebAPI/Program.cs
using HatsuneMikuShopWebAPI.Models;
========
using HatsuneMikuShopAPI.Models;
>>>>>>>> abfe7156f0424137ca89cb0a14e9dcaa344ae50c:HatsuneMikuShopAPI/HatsuneMikuShopAPI/Program.cs
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
var builder = WebApplication.CreateBuilder(args);

// �`�JMikuMusicShopContext�����O
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
//��ȤW�ä��|�ϥ��R�A�ɮסA�]��API�q�`�O���ѵ��e�ݨϥΪ��A�e�ݷ|���ۤv���R�A�ɮ׳B�z�覡
//app.UseStaticFiles();

========
>>>>>>>> abfe7156f0424137ca89cb0a14e9dcaa344ae50c:HatsuneMikuShopAPI/HatsuneMikuShopAPI/Program.cs
app.UseAuthorization();

app.MapControllers();

app.Run();
