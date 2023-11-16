using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using SimpleWebAPI.Controllers;
using SimpleWebAPI.Services;

namespace SimpleWebAPI.Test
{
    [TestFixture]
    public class WeatherForecastTests
    {
        private IList<DateTime>? _datePeriod;
        private IList<DateTime>? _differentDays;

        private WeatherForecastController? _weatherForecastController;

        [SetUp]
        public void SetUp()
        {
            IWeatherStoringService weatherStoringService = new WeatherStoringService();
            ILogger<WeatherForecastController> logger = new NullLogger<WeatherForecastController>();
            _weatherForecastController = new WeatherForecastController(logger, weatherStoringService);
            _datePeriod = new List<DateTime>();
            _differentDays = new List<DateTime>();
            Random random = new();
            for (DateTime i = DateTime.Today.AddYears(-2); i < DateTime.Today; i = i.AddDays(1))
            {
                _datePeriod.Add(i);
            }

            for (int i = 0; i < 10000; i++)
            {
                _differentDays.Add(DateTime.Today.AddDays(random.Next(- 1500, 0)));
            }

            _differentDays = _differentDays.Distinct().ToList();
        }

        [Test]
        public void CheckDefaultData()
        {
            if (_datePeriod != null)
            {
                Assert.IsTrue(_weatherForecastController?.GetAll().Any(x => _datePeriod.Contains(x.Date)),
                    "Default data should contain data for last two years");
            }
        }

        [Test]
        public void CheckDataForDifferentDays()
        {
            if (_differentDays != null)
            {
                foreach (var day in _differentDays)
                {
                    if (day < DateTime.Today.AddYears(-2))
                    {
                        Assert.IsNull(_weatherForecastController?.Get(day),
                            $"Default data should contain data only for last two years; {day.Date.ToShortDateString()}");
                    }
                    else
                    {
                        Assert.IsNotNull(_weatherForecastController?.Get(day),
                            $"Default data should contain data for last two years; {day.Date.ToShortDateString()}");
                    }
                }
            }
        }

        [Test]
        public void CheckAddLogic()
        {
            var testSummary = "TestSummary";
            var testTemperature = 20;
            if (_differentDays != null)
            {
                foreach (var day in _differentDays)
                {
                    if (day < DateTime.Today.AddYears(-2))
                    {
                        Assert.IsNotNull(_weatherForecastController?.AddNewWeather(testSummary, day, testTemperature),
                            $"Default data should contain data only for last two years; {day.Date.ToShortDateString()}");
                    }
                    else
                    {
                        Assert.IsNull(_weatherForecastController?.AddNewWeather(testSummary, day, testTemperature),
                            $"Default data should contain data for last two years; {day.Date.ToShortDateString()}");
                    }
                }
            }
        }

        [Test]
        public void CheckUpdateLogic()
        {
            var testSummary = "TestSummary";
            var testTemperature = 20;
            if (_differentDays != null)
            {
                foreach (var day in _differentDays)
                {
                    if (day < DateTime.Today.AddYears(-2))
                    {
                        Assert.IsNull(_weatherForecastController?.UpdateWeather(testSummary, day, testTemperature),
                            $"Default data contains data only for last two years; {day.Date.ToShortDateString()}");
                    }
                    else
                    {
                        Assert.IsNotNull(_weatherForecastController?.UpdateWeather(testSummary, day, testTemperature),
                            $"Default data should contain data for last two years; {day.ToShortDateString()}");
                        var newValue = _weatherForecastController?.Get(day);
                        Assert.IsTrue(newValue?.Date == day && newValue.Summary == testSummary && newValue.TemperatureC == testTemperature, "Update work not as expected");
                    }
                }
            }
        }

        [Test]
        public void CheckDeleteLogic()
        {
            if (_differentDays != null)
            {
                foreach (var day in _differentDays)
                {
                    if (day < DateTime.Today.AddYears(-2))
                    {
                        Assert.IsNull(_weatherForecastController?.DeleteWeather(day),
                            $"Default data contains data only for last two years; {day.Date.ToShortDateString()}");
                    }
                    else
                    {
                        Assert.IsNotNull(_weatherForecastController?.DeleteWeather(day),
                            $"Default data should contain data for last two years; {day.Date.ToShortDateString()}");
                        Assert.IsNull(_weatherForecastController?.Get(day), $"Data for {day.ToShortDateString()} still exists");
                        
                    }
                }
            }
        }
    }
}
