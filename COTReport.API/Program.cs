using COTReport.Common.Helper;
using COTReport.DAL.Context;
using COTReport.DAL.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ReportDbContext>();
builder.Services.AddScoped<ReportRepository>();
builder.Services.AddScoped<MyFxbookHelper>();

builder.Services.AddMemoryCache();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
