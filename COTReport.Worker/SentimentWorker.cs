using COTReport.Common.Exceptions;
using COTReport.Common.Helper;
using COTReport.Common.Mapper;
using COTReport.Common.Model;
using COTReport.DAL.Repository;

namespace COTReport.Worker
{
    public class SentimentWorker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly MyFxbookHelper _myFxbookHelper;
        private readonly SentimentRepository _sentimentRepo;

        public SentimentWorker(ILogger<SentimentWorker> logger, MyFxbookHelper myFxbookHelper, SentimentRepository sentimentRepository)
        {
            _logger = logger;
            _myFxbookHelper = myFxbookHelper;
            _sentimentRepo = sentimentRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("=> Worker running now...");
                    MyFxbookmodel data = await _myFxbookHelper.GetSeniments();
                    if (data != null && data?.Symbols?.Count > 0)
                    {
                        await _sentimentRepo.SaveRecords(data.Symbols.ToEntity());
                    }
                }
                catch (ExternalApiException ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }

                await Task.Delay(TimeSpan.FromMinutes(30));
            }
        }

    }
}