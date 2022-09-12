using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using ShareBear.Helpers;

namespace ShareBear.Services
{
    public interface IFileService
    {

    }

    public class FileService : IFileService
    {
        private BlobContainerClient _blobContainerClient;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IOptions<AppSettings> appSettings;
        private readonly IWebHostEnvironment env;

        public FileService(IOptions<AppSettings> appSettings, IWebHostEnvironment env)
        {
            this.appSettings = appSettings;
            this.env = env;
            var connectionString = appSettings.Value.AzureStorageConnectionString;

            this._blobServiceClient = new BlobServiceClient(connectionString);


            if(env.IsProduction())
            {
                this._blobContainerClient = _blobServiceClient.GetBlobContainerClient("production-sharebear-container");
            }
            else
            {
                this._blobContainerClient = _blobServiceClient.GetBlobContainerClient("development-sharebear-container");
            }
        }

        private async Task ValidateBlobContainer()
        {
            if(!await _blobContainerClient.ExistsAsync())
            {
                if (env.IsProduction())
                {
                    this._blobContainerClient = 
                        await _blobServiceClient.CreateBlobContainerAsync("production-sharebear-container");
                }
                else
                {
                    this._blobContainerClient = 
                        await _blobServiceClient.CreateBlobContainerAsync("development-sharebear-container");
                }
            }
        }

        public async Task UploadFile(IFormFile file)
        {
            await ValidateBlobContainer();

            var blob = _blobContainerClient.GetBlobClient("");

            using var fileData = file.OpenReadStream();

            var result = await blob.UploadAsync(fileData);
        }


        public async Task TestingMethod()
        {
            BlobContainerClient containerClient = 
                await _blobServiceClient.CreateBlobContainerAsync("TestingContainer" + Guid.NewGuid().ToString());
            
            var blobClient = containerClient.GetBlobClient("TestingName" + Guid.NewGuid().ToString() + ".txt");

            Console.WriteLine(blobClient.Uri);

        }


        public void UploadDocument()
        {

        }
    }
}
