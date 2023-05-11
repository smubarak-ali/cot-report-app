// See https://aka.ms/new-console-template for more information
using COTReport.Common.Helper;
using COTReport.DAL.Context;
using COTReport.DAL.Repository;
using COTReport.Worker;
using Serilog;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices(services =>
        {
            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console(outputTemplate: "{Timestamp:yy-MM-dd HH:mm} [{Level:u3}] - {Message}{NewLine}{Exception}")
                    .CreateLogger();

            services.AddDbContext<ReportDbContext>();
            services.AddSingleton<MyFxbookHelper>();
            services.AddSingleton<SentimentRepository>();

            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = Environment.GetEnvironmentVariable("REDIS_HOST");
            });

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddHostedService<SentimentWorker>();
        })
        .Build();

        await host.RunAsync();
    }
}