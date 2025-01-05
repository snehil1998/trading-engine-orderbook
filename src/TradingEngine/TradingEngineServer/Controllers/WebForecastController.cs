using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TradingEngineServer.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebForecastController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "Sunny", "Cloudy", "Rainy" };
        }
    }
}
