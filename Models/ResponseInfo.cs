namespace Hackathon_API.Models
{
    public class ResponseInfo
    {
        public AirportDelay Delay { get; set; }
        public WeatherInfo Weather { get; set; }
        //public object Delays { get; set; }
        public SafetyIndexInfo Safety { get; set; }
        public HealthInfo Health { get; set; }
    }
}
