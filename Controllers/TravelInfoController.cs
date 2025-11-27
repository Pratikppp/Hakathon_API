using Hackathon_API.Models;
using Hackathon_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelInfoController : ControllerBase
    {
        private readonly WeatherForecastServices _weatherService;
        private readonly HealthRiskService _healthService;
        private readonly SafetyService _safetyService;
        private readonly AeroDataBoxService _delayService;

        public TravelInfoController(WeatherForecastServices weatherService, HealthRiskService healthService, 
            SafetyService safetyService, AeroDataBoxService delayService)
        {
            _weatherService = weatherService;
            _healthService = healthService;
            _safetyService = safetyService;
            _delayService = delayService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(string country, string date)
        {
            var health = await _healthService.GetHealthAsync(country);
            var delay = await _delayService.GetAirportDelays(country, date);
            var weather = await _weatherService.GetWeatherForecast(country, date);


            var safety = await _safetyService.GetSafetyAsync(country);

            var response = new ResponseInfo
            {
                Weather = weather,
                Delay = delay,
                Safety = safety,
                Health = health
            };
                
            return Ok(response);
        }

    }
}
