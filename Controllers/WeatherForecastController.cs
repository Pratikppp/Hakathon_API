using Hackathon_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherForecastServices _weatherForecastService;

        public WeatherForecastController(WeatherForecastServices weatherForecastService)
        {
            _weatherForecastService = weatherForecastService;
        }

        // GET: /api/weather/India/2025-11-19T13:00:00
        [HttpGet("{location}/{date}")]
        public async Task<IActionResult> GetForecast(string location, string date)
        {
            var result = await _weatherForecastService.GetWeatherForecast(location, date);
            return Ok();
        }
    }
}
