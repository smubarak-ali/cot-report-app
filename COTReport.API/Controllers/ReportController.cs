using COTReport.DAL.Entity;
using COTReport.DAL.Repository;
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
        private readonly IDistributedCache _cache;

        public ReportController(ReportRepository reportRepository, ILogger<ReportController> logger, IDistributedCache cache, SentimentRepository sentimentRepo)
        {
            _reportRepo = reportRepository;
            _sentimentRepo = sentimentRepo;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet("cot")]
        public async Task<IActionResult> GetCotReport()
        {
            try
            {
                string cacheKey = "cot";
                var redisList = await _cache.GetStringAsync(cacheKey);
                if (redisList == null)
                {
                    var list = await _reportRepo.GetReportAsync();
                    var groupedList = list.GroupBy(x => x.Code).Select(x =>
                    {
                        return x.First();
                    });

                    var responseList = groupedList.OrderBy(x => x.Code);
                    var cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(180));
                    await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(responseList), cacheEntryOptions);
                    return Ok(responseList);
                }

                var cachedListForResponse = JsonConvert.DeserializeObject<IEnumerable<Report>>(redisList);
                return Ok(cachedListForResponse);
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
                var redisList = await _cache.GetStringAsync(cacheKey);
                if (redisList == null)
                {
                    var list = await _reportRepo.GetReportByCodeAsync(code);
                    var cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(180));
                    await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(list), cacheEntryOptions);
                    return Ok(list);
                }

                var responseList = JsonConvert.DeserializeObject<List<Report>>(redisList);
                return Ok(responseList);
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
                var redisList = await _cache.GetStringAsync(cacheKey);
                if (redisList == null)
                {
                    var list = await _sentimentRepo.GetSentimentsAsync();
                    var cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(15));
                    await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(list), cacheEntryOptions);
                    return Ok(new { Symbols = list });
                }

                var responseList = JsonConvert.DeserializeObject<List<Sentiment>>(redisList);
                return Ok(new { Symbols = responseList });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
    }
}