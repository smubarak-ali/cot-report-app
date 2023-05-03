// See https://aka.ms/new-console-template for more information
using COTReport.Common.Helper;
using COTReport.DAL.Context;
using COTReport.DAL.Repository;
using COTReport.Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<ReportDbContext>();
        services.AddSingleton<MyFxbookHelper>();
        services.AddSingleton<SentimentRepository>();
        services.AddLogging(opt => opt.AddConsole());

        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = Environment.GetEnvironmentVariable("REDIS_HOST");
        });

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddHostedService<SentimentWorker>();
    })
    .Build();

await host.RunAsync();