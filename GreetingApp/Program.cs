using BusinessLayer.Interface;
using BusinessLayer.Service;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using RepositoryLayer.Context;
using NLog;
using NLog.Web;
using Microsoft.EntityFrameworkCore;
using Middleware.GlobalExceptionHandler;
var builder = WebApplication.CreateBuilder(args);
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("Application Starting...");
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<GreetingAppContext>(options => options.UseSqlServer(connectionString));
// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>(); // Register the global exception filter
});
builder.Services.AddScoped<IGreetingBL, GreetingBL>();
builder.Services.AddScoped<IGreetingRL, GreetingRL>();

// Add NLog to the service collection
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
builder.Host.UseNLog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
