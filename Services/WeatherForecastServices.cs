using Hackathon_API.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
namespace Hackathon_API.Services
{
    public class WeatherForecastServices
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public WeatherForecastServices(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<WeatherInfo> GetWeatherForecast(string country, string date)
        {
            string baseUrl = _config["VisualCrossing:BaseUrl"];
            string apiKey = _config["VisualCrossing:ApiKey"];

            string url = $"{baseUrl}/{country}/{date}?key={apiKey}&include=days";

            var response = await _http.GetStringAsync(url);

            using JsonDocument json = JsonDocument.Parse(response);

            var current = json.RootElement.GetProperty("days");
            string conditions = null;
            string description = null;
            string icon = null;
            // var forecast = json.RootElement.GetProperty("forecast").GetProperty("forecastday")[0].GetProperty("day");
            foreach (var day in current.EnumerateArray())
            {
                conditions = day.GetProperty("conditions").GetString();
                description = day.GetProperty("description").GetString();
                icon = day.GetProperty("icon").GetString();
            }
            return new WeatherInfo
            {
                Conditions = conditions,
                Description = description,
                Icon = icon
            };
        }
    }
}
