namespace SimpleWebAPI.Services
{
    public class WeatherStoringService : IWeatherStoringService
    {
        public WeatherStoringService()
        {
            Weather = GenerateWeatherForLastTwoYears();
        }
        
        public IList<WeatherForecast> Weather { get; set; }

        private readonly IList<string> _summaries = new List<string>
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private IList<WeatherForecast> GenerateWeatherForLastTwoYears()
        {
            var result = new List<WeatherForecast>();
            Random random = new();
            for (DateTime i = DateTime.Today.AddYears(-2); i < DateTime.Today; i = i.AddDays(1))
            {
                result.Add(new WeatherForecast
                {
                    Date = i,
                    Summary = _summaries[random.Next(0, 9)],
                    TemperatureC = random.Next(-20, 20)
                });
            }

            return result;
        }
    }
}
