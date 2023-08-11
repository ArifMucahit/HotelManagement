using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReportManagementAPI.Middleware;
using ReportManagementAPI.Models;
using ReportManagementAPI.Repositories;
using ReportManagementAPI.Repositories.Repositories;
using ReportManagementAPI.Repositories.Repositories.Interface;
using ReportManagementAPI.Services;
using ReportManagementAPI.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var services = builder.Services;

var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new ReportManagementAPI.Models.Mapper.MapperConfiguration() ); });

services.AddSingleton(mappingConfig.CreateMapper());

services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitConf"));

services.Configure<ElasticsearchOptions>(builder.Configuration.GetSection("Elastic"));

var conStr = builder.Configuration.GetConnectionString("Postgres");
services.AddDbContext<ReportManagementContext>(opt => 
    opt.UseNpgsql(conStr));

services.AddTransient<IQueueService, QueueService>();
services.AddTransient<ILogManager, LogManager>();
services.AddTransient<IReportService, ReportService>();

services.AddTransient<IReportRepository, ReportRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();