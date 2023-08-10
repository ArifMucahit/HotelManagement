using AutoMapper;
using HotelManagementAPI.Middlewares;
using HotelManagementAPI.Models;
using HotelManagementAPI.Repositories;
using HotelManagementAPI.Repositories.Repository;
using HotelManagementAPI.Repositories.Repository.Interface;
using HotelManagementAPI.Services;
using HotelManagementAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new HotelManagementAPI.Models.Mapper.MapperConfiguration()); });

services.AddSingleton(mappingConfig.CreateMapper());

services.AddSingleton<IConnectionMultiplexer>(x =>
{
    var redisConfig = x.GetRequiredService<IOptions<RedisConfiguration>>().Value;
    ConfigurationOptions options = new ConfigurationOptions
    {
        EndPoints = { $"{redisConfig.Host}:{redisConfig.Port}" },
        Password = redisConfig.Password,
        User = redisConfig.Username,
        AbortOnConnectFail = false
    };
    return ConnectionMultiplexer.Connect(options);
});

var conStr = builder.Configuration.GetConnectionString("Postgres");
services.AddDbContext<HotelManagementContext>(opt => 
    opt.UseNpgsql(conStr));

services.AddSingleton<ICacheManager, CacheManager>();


services.AddTransient<IHotelRepository, HotelRepository>();
services.AddTransient<IHotelManagerRepository, HotelManagerRepository>();
services.AddTransient<IContactInfoRepository, ContactInfoRepository>();


services.AddTransient<IHotelService, HotelService>();


var app = builder.Build();

app.UseHttpsRedirection();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();
app.UseAuthorization();

app.MapControllers();

app.Run();