using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace VSAng.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        ILogger _log; 
        public SampleDataController(ILogger<SampleDataController> log)
        {
            _log = log;
        }
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            _log.LogCritical("building weather forecast");

            var rng = new Random();

            return Enumerable.Range(1, 5).Select(index => {
                int temperatureC = rng.Next(-20, 55);
                return (new WeatherForecast
                    {
                        DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                        TemperatureC = temperatureC,
                        Summary = Summaries[(Int16)((temperatureC + 20) / 10)]
                    });
                }
            );
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}
