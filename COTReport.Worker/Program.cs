// See https://aka.ms/new-console-template for more information
using COTReport.Common.Exceptions;
using COTReport.Common.Helper;
using COTReport.Common.Model;
using COTReport.DAL.Context;
using COTReport.DAL.Repository;
using COTReport.Common.Mapper;
using Serilog;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console(outputTemplate: "{Timestamp:yy-MM-dd HH:mm} [{Level:u3}] - {Message}{NewLine}{Exception}")
                    .CreateLogger();

        var services = new ServiceCollection();
        services.AddDbContext<ReportDbContext>();
        services.AddSingleton<MyFxbookHelper>();
        services.AddSingleton<SentimentRepository>();

        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = Environment.GetEnvironmentVariable("REDIS_HOST");
        });

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        ServiceProvider serviceProvider = services.BuildServiceProvider();

        try
        {
            var _sentimentRepo = serviceProvider.GetService<SentimentRepository>();
            var _myFxbookHelper = serviceProvider.GetService<MyFxbookHelper>();

            Log.Information("=> Worker running now...");
            MyFxbookmodel data = await _myFxbookHelper.GetSeniments();
            if (data != null && data?.Symbols?.Count > 0)
            {
                await _sentimentRepo.SaveRecords(data.Symbols.ToEntity());
            }
        }
        catch (ExternalApiException ex)
        {
            Log.Error(ex.Message, ex);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message, ex);
        }
    }
}