using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShareBear.Helpers;

namespace ShareBear.Controllers
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
        private readonly IOptions<AppSettings> appSettings;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            this.appSettings = appSettings;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [Route("uploadImage")]
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] List<IFormFile> files)
        {
            try
            {
                if (files.Count == 0)
                    return BadRequest();


                var file = files[0];


                var blobServiceClient = new BlobServiceClient(appSettings.Value.AzureStorageConnectionString);

                var containerClient = blobServiceClient.GetBlobContainerClient("testingcontainer");
                
                if (!await containerClient.ExistsAsync())
                {
                    containerClient =
                        await blobServiceClient.CreateBlobContainerAsync("testingcontainer");
                }

                
                BlobClient blob = containerClient.GetBlobClient(file.FileName);

                using var fileData = file.OpenReadStream();

                var result = await blob.UploadAsync(fileData);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Ok();
            }
        }
    }
}