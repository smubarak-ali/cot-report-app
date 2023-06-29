using COTReport.DAL.Entity;
using COTReport.DAL.Repository;
using COTReport.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace COTReport.API.Controllers
{
    [ApiController]
    [Route("/api/report")]
    public class ReportController : ControllerBase
    {
        private readonly ReportRepository _reportRepo;
        private readonly SentimentRepository _sentimentRepo;
        private readonly ILogger _logger;
        //private readonly IDistributedCache _cache;
        private readonly ICacheService<List<Report>> _reportCacheService;
        private readonly ICacheService<List<Sentiment>> _sentimentCacheService;

        public ReportController(
            ReportRepository reportRepository,
            ILogger<ReportController> logger,
            SentimentRepository sentimentRepo,
            ICacheService<List<Report>> reportCacheService,
            ICacheService<List<Sentiment>> sentimentCacheService)
        {
            _reportRepo = reportRepository;
            _sentimentRepo = sentimentRepo;
            _logger = logger;
            _reportCacheService = reportCacheService;
            _sentimentCacheService = sentimentCacheService;
            //_cache = cache;
        }

        [HttpGet("cot")]
        public async Task<IActionResult> GetCotReport()
        {
            try
            {
                string cacheKey = "cot";
                var redisList = await _reportCacheService.GetDataAsync(cacheKey);
                if (redisList == default)
                {
                    var list = await _reportRepo.GetReportAsync();
                    var groupedList = list.GroupBy(x => x.Code).Select(x =>
                    {
                        return x.First();
                    });

                    var responseList = groupedList.OrderBy(x => x.Code).ToList();
                    var cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(180)).SetSlidingExpiration(TimeSpan.FromMinutes(60));
                    await _reportCacheService.SaveInCacheAsync(cacheKey, responseList, absoluteExpiration: 180, slidingExpiration: 60);
                    return Ok(responseList);
                }

                
                return Ok(redisList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("cot/{code}")]
        public async Task<IActionResult> GetCotReportByCode(string code)
        {
            try
            {
                string cacheKey = code;
                var redisList = await _reportCacheService.GetDataAsync(cacheKey);
                if (redisList == default)
                {
                    var list = await _reportRepo.GetReportByCodeAsync(code);
                    var cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(120)).SetSlidingExpiration(TimeSpan.FromMinutes(60));
                    await _reportCacheService.SaveInCacheAsync(cacheKey, list, absoluteExpiration: 120, slidingExpiration: 60);
                    return Ok(list);
                }

                return Ok(redisList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("sentiment")]
        public async Task<IActionResult> GetSentimentReport()
        {
            try
            {
                string cacheKey = "sentiments";
                var redisList = await _sentimentCacheService.GetDataAsync(cacheKey);
                if (redisList == default)
                {
                    var list = await _sentimentRepo.GetSentimentsAsync();
                    var cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(7)).SetSlidingExpiration(TimeSpan.FromMinutes(5));
                    await _sentimentCacheService.SaveInCacheAsync(cacheKey, list);
                    return Ok(new { Symbols = list });
                }

                
                return Ok(new { Symbols = redisList });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
    }
}