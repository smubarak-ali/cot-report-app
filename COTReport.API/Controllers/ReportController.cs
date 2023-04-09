
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

        public ReportController(ReportRepository reportRepository)
        {
            _reportRepo = reportRepository;
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
        public IActionResult GetSentimentReport()
        {
            try
            {

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}