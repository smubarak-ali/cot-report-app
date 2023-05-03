using COTReport.Common.Helper;
using COTReport.DAL.Context;
using COTReport.DAL.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

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

builder.Services.AddControllers();

var app = builder.Build();
app.UseCors("MyLocal");
app.MapControllers();
app.Run();
