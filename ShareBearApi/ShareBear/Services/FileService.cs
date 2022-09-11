using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using ShareBear.Helpers;

namespace ShareBear.Services
{
    public class FileService
    {
        private readonly BlobServiceClient blobServiceClient;
        private readonly IOptions<AppSettings> appSettings;

        public FileService(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings;

            var connectionString = appSettings.Value.AzureStorageConnectionString;

            this.blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task TestingMethod()
        {
            BlobContainerClient containerClient = 
                await blobServiceClient.CreateBlobContainerAsync("TestingContainer" + Guid.NewGuid().ToString());
            
            var blobClient = containerClient.GetBlobClient("TestingName" + Guid.NewGuid().ToString() + ".txt");

            Console.WriteLine(blobClient.Uri);

        }


        public void UploadDocument()
        {

        }
    }
}
