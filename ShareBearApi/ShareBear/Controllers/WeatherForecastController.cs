using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using ShareBear.Data.Models.FingerprintJs;
using ShareBear.Filters;
using ShareBear.Helpers;
using System.Text.Json;

namespace ShareBear.Controllers
{



    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IOptions<AppSettings> appSettings;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            this.appSettings = appSettings;
        }

        [TypeFilter(typeof(VisitorAuthorizeFilter))]
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var visitorId = HttpContext.GetVisitorId();


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}