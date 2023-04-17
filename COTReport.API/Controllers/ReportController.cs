
using COTReport.Common.Exceptions;
using COTReport.Common.Helper;
using COTReport.DAL.Repository;
using Microsoft.AspNetCore.Mvc;

namespace COTReport.API.Controllers
{
    [ApiController]
    [Route("/api/report")]
    public class ReportController : ControllerBase
    {
        private readonly ReportRepository _reportRepo;
        private readonly MyFxbookHelper _myFxbookHelper;
        private readonly ILogger _logger;

        public ReportController(ReportRepository reportRepository, MyFxbookHelper myFxbookHelper, ILogger<ReportController> logger)
        {
            _reportRepo = reportRepository;
            _myFxbookHelper = myFxbookHelper;
            _logger = logger;
        }

        [HttpGet("cot")]
        public IActionResult GetCotReport()
        {
            try
            {
                var list = _reportRepo.GetReport();
                var groupedList = list.GroupBy(x => x.Code).Select(x =>
                {
                    return x.First();
                });
                return Ok(groupedList);
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