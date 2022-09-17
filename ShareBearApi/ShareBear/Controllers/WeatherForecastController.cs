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



                StorageSharedKeyCredential storageSharedKeyCredential = 
                    new StorageSharedKeyCredential(blobServiceClient.AccountName, appSettings.Value.AzureStorageAccessKey);

           
                //  Defines the resource being accessed and for how long the access is allowed.
                var blobSasBuilder = new BlobSasBuilder
                {
                    //StartsOn = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(15d)),
                    ExpiresOn = DateTime.UtcNow.AddDays(15),
                    BlobContainerName = "testingcontainer",


                    // If you omit this, it's going to create an sas for every container item
                    //BlobName = "test.png",
                };

                //  Defines the type of permission.
                blobSasBuilder.SetPermissions(BlobSasPermissions.Write | BlobSasPermissions.Read | BlobSasPermissions.List);


                //  Builds the Sas URI.
                BlobSasQueryParameters sasQueryParameters = blobSasBuilder.ToSasQueryParameters(storageSharedKeyCredential);
                UriBuilder sasUri = new UriBuilder(blob.Uri);
                sasUri.Query = sasQueryParameters.ToString();

                var g = containerClient.Uri;
                return Ok(sasUri);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Ok();
            }
        }

        /// <summary>
        /// Returns a URI containing a SAS for the blob container.
        /// </summary>
        /// <param name="container">A reference to the container.</param>
        /// <param name="StorageSharedKeyCredential">Storage Shared Key Credential.</param>
        /// <returns>A string containing the URI for the container, with the SAS token appended.</returns>
        static UriBuilder GetContainerSasUri(BlobContainerClient container, StorageSharedKeyCredential storageSharedKeyCredential)
        {
            var policy = new BlobSasBuilder
            {
                BlobContainerName = container.Name,
                Resource = "c",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
            };
            policy.SetPermissions(BlobSasPermissions.All);
            var sas = policy.ToSasQueryParameters(storageSharedKeyCredential).ToString();
            UriBuilder sasUri = new UriBuilder(container.Uri);
            sasUri.Query = sas;
            //Return the URI string for the container, including the SAS token.
            return sasUri;
        }
    }
}