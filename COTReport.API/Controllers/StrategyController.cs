
using Microsoft.AspNetCore.Mvc;

namespace COTReport.API.Controllers
{
    [ApiController]
    [Route("/api/strategy")]
    public class StrategyController : ControllerBase
    {
        private readonly ILogger _logger;

        public StrategyController(ILogger<StrategyController> logger)
        {
            _logger = logger;
        }

        public IActionResult Get()
        {
            var list = new List<dynamic>();
            list.Add(new
            {
                Id = 1,
                Title = "EMA based strategy",
                Filename = "/strategy/EMA_Based.html",
                VideoUrl = ""
            });
            list.Add(new
            {
                Id = 2,
                Title = "20 EMA strategy",
                Filename = "/strategy/1H_20_EMA.html",
                VideoUrl = ""
            });

            return Ok(list);
        }
    }
}