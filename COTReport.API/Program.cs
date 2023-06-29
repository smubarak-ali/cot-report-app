using COTReport.Common.Helper;
using COTReport.DAL.Context;
using COTReport.DAL.Entity;
using COTReport.DAL.Repository;
using COTReport.Service.Implementation;
using COTReport.Service.Interface;
using Serilog;

var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm} [{Level:u3}] - {Message}{NewLine}{Exception}")
                .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("MyLocal", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = Environment.GetEnvironmentVariable("REDIS_HOST");
});

builder.Services.AddDbContext<ReportDbContext>();
builder.Services.AddScoped<ReportRepository>();
builder.Services.AddScoped<SentimentRepository>();
builder.Services.AddScoped<MyFxbookHelper>();
builder.Services.AddSingleton(typeof(ICacheService<>), typeof(CacheService<>));
builder.Services.AddScoped<ICacheService<List<Report?>>, ReportCacheService>();
builder.Services.AddScoped<ICacheService<List<Sentiment?>>, SentimentCacheService>();

builder.Services.AddControllers();

var app = builder.Build();
app.UseCors("MyLocal");
app.UseStaticFiles();
app.MapControllers();
app.Run();
