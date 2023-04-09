
using COTReport.Common.Helper;
using COTReport.DAL.Entity;
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

        public ReportController(ReportRepository reportRepository, MyFxbookHelper myFxbookHelper)
        {
            _reportRepo = reportRepository;
            _myFxbookHelper = myFxbookHelper;
        }

        [HttpGet("cot")]
        public IActionResult GetCotReport()
        {
            try
            {
                var list = _reportRepo.GetReport();
                var groupedList = list.GroupBy(x => x.Code).Select(x =>
                {
                    var dict = new Dictionary<string, List<Report>>();
                    dict.Add(x.Key, x.ToList());
                    return dict;
                });
                return Ok(groupedList);
            }
            catch (Exception ex)
            {
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}