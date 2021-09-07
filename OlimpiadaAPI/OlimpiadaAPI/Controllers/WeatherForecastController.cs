using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCaching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace OlimpiadaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDistributedCache cache;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDistributedCache cache)
        {
            _logger = logger;
            this.cache = cache;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            const string cacheKey = "WeatherForecastController.Get";
            var rng = new Random();

            var cacheValue = cache.Get(cacheKey);
            WeatherForecast[] ret;

            if ((cacheValue?.Length ?? 0) == 0)
            {
                ret = Enumerable.Range(1, 150).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                 .ToArray();

                var values = JsonSerializer.Serialize(ret);

                cache.Set(cacheKey, Encoding.UTF8.GetBytes(values), new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });
                Thread.Sleep(500);
            }
            else
            {
                var json = Encoding.UTF8.GetString(cacheValue);
                ret = JsonSerializer.Deserialize<WeatherForecast[]>(json);
            }




            return ret;
        }
    }
}
