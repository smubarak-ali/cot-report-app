
using COTReport.Common.Exceptions;
using COTReport.Common.Helper;
using COTReport.DAL.Entity;
using COTReport.DAL.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace COTReport.API.Controllers
{
    [ApiController]
    [Route("/api/report")]
    public class ReportController : ControllerBase
    {
        private readonly ReportRepository _reportRepo;
        private readonly MyFxbookHelper _myFxbookHelper;
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache;

        public ReportController(ReportRepository reportRepository, MyFxbookHelper myFxbookHelper, ILogger<ReportController> logger, IMemoryCache cache)
        {
            _reportRepo = reportRepository;
            _myFxbookHelper = myFxbookHelper;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet("cot")]
        public IActionResult GetCotReport()
        {
            try
            {
                if (!_cache.TryGetValue("cot", out string cachedList))
                {
                    var list = _reportRepo.GetReport();
                    var groupedList = list.GroupBy(x => x.Code).Select(x =>
                    {
                        return x.First();
                    });

                    var responseList = groupedList.OrderBy(x => x.Code);
                    _cache.Set("cot", JsonConvert.SerializeObject(responseList), absoluteExpirationRelativeToNow: TimeSpan.FromHours(3));
                    return Ok(responseList);
                }

                var cachedListForResponse = JsonConvert.DeserializeObject<IEnumerable<Report>>(cachedList);
                return Ok(cachedListForResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("cot/{code}")]
        public IActionResult GetCotReportByCode(string code)
        {
            try
            {
                if (!_cache.TryGetValue(code, out string cachedCotList))
                {
                    var list = _reportRepo.GetReportByCode(code);
                    _cache.Set(code, JsonConvert.SerializeObject(list), absoluteExpirationRelativeToNow: TimeSpan.FromHours(3));
                    return Ok(list);
                }

                var responseList = JsonConvert.DeserializeObject<List<Report>>(cachedCotList);
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
                var obj = await _myFxbookHelper.GetSeniments();
                return Ok(obj);
            }
            catch (ExternalApiException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}