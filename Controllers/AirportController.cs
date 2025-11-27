using Hackathon_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon_API.Controllers
{
    public class AirportController : ControllerBase
    {
        private readonly AeroDataBoxService _service;

        public AirportController(AeroDataBoxService service)
        {
            _service = service;
        }

        //[HttpGet("delays/{icao}")]
        //public async Task<IActionResult> GetDelays(string icao)
        //{
        //    var data = await _service.GetAirportDelays(icao);
        //    return Ok(data);
        //}   
    }
}
