using COTReport.DAL.Context;
using COTReport.DAL.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ReportDbContext>();
builder.Services.AddScoped<ReportRepository>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
