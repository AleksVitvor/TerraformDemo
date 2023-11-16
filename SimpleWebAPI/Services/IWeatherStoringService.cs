namespace SimpleWebAPI.Services
{
    public interface IWeatherStoringService
    {
        IList<WeatherForecast> Weather { get; set; }
    }
}
