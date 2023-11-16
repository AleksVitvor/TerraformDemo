using Microsoft.AspNetCore.Mvc;
using SimpleWebAPI.Services;

namespace SimpleWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherStoringService _weatherStoringService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherStoringService weatherStoringService)
        {
            _logger = logger;
            _weatherStoringService = weatherStoringService;
        }

        [HttpGet("{date}")]
        public WeatherForecast? Get(DateTime date)
        {
            _logger.Log(LogLevel.Information, $"Get by date {date.ToShortDateString()} started");
            var result = _weatherStoringService.Weather.FirstOrDefault(x => x.Date == date);
            _logger.Log(LogLevel.Information, $"Get by date {date.ToShortDateString()} success");
            return result;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> GetAll()
        {
            _logger.Log(LogLevel.Information, $"Get all started");
            var result = _weatherStoringService.Weather;
            _logger.Log(LogLevel.Information, $"Get all success");
            return result;
        }

        [HttpPost]
        public WeatherForecast? AddNewWeather(string summary, DateTime date, int temperature)
        {
            _logger.Log(LogLevel.Information, $"Add new started");
            var newWeather = new WeatherForecast
            {
                Date = date,
                Summary = summary,
                TemperatureC = temperature
            };

            if (_weatherStoringService.Weather.All(x => x.Date != date))
            {
                _weatherStoringService.Weather.Add(newWeather);
                _logger.Log(LogLevel.Information, $"Add new success");
                return newWeather;
            }

            _logger.Log(LogLevel.Information, $"Add new success");
            return null;
        }

        [HttpPut]
        public WeatherForecast? UpdateWeather(string summary, DateTime date, int temperature)
        {
            _logger.Log(LogLevel.Information, $"Update started");
            var newWeather = new WeatherForecast
            {
                Date = date,
                Summary = summary,
                TemperatureC = temperature
            };

            if (_weatherStoringService.Weather.Any(x => x.Date == date))
            {
                _weatherStoringService.Weather.Remove(
                    _weatherStoringService.Weather.First(x => x.Date == date));
                _weatherStoringService.Weather.Add(newWeather);
                _logger.Log(LogLevel.Information, $"Update success");
                return newWeather;
            }

            _logger.Log(LogLevel.Information, $"Update success");
            return null;
        }

        [HttpDelete]
        public WeatherForecast? DeleteWeather(DateTime date)
        {
            _logger.Log(LogLevel.Information, $"Delete started");

            if (_weatherStoringService.Weather.Any(x => x.Date == date))
            {
                var oldWeather = _weatherStoringService.Weather.First(x => x.Date == date);
                _weatherStoringService.Weather.Remove(oldWeather);
                _logger.Log(LogLevel.Information, $"Delete success");
                return oldWeather;
            }

            _logger.Log(LogLevel.Information, $"Delete success");
            return null;
        }
    }
}