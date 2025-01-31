using System.Reflection;
using System.Text.Json.Serialization;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using UnitTestExample.BackgroundServices;
using UnitTestExample.Data;
using UnitTestExample.Extensions;
using UnitTestExample.Hubs;
using UnitTestExample.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(config =>
{
    config.UseMemoryStorage();
});

builder.Services.AddHangfireServer();
builder.Services.AddServicesAutomatically(Assembly.GetExecutingAssembly());

//builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddSingleton<ITelegramBotClient>(sp =>
    new TelegramBotClient("6336709961:AAEJoxZCFUqqXzrz-WLJpCrpJ-SHHNZJ-dQ"));

builder.Services.AddScoped<TelegramUserService>();
builder.Services.AddHostedService<TelegramBackgroundService>();

builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});

var app = builder.Build();

app.UseCors(configurePolicy: config =>
{
    config.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHub<QuestionHub>("/chatHub");

app.Run();