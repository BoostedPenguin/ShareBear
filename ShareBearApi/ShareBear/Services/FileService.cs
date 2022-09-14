using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Myrmec;
using ShareBear.Helpers;

namespace ShareBear.Services
{
    public interface IFileService
    {
        Task UploadFile(string containerName, string fileName, IFormFile file);
    }

    public class InvalidFileTypeException : Exception
    {
        public InvalidFileTypeException(string message) : base(message)
        {

        }
    }

    public class FileService : IFileService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IOptions<AppSettings> appSettings;
        private readonly IWebHostEnvironment env;
        private readonly ILogger<FileService> logger;

        public FileService(IOptions<AppSettings> appSettings, IWebHostEnvironment env, ILogger<FileService> logger)
        {
            this.appSettings = appSettings;
            this.env = env;
            this.logger = logger;
            var connectionString = appSettings.Value.AzureStorageConnectionString;

            this._blobServiceClient = new BlobServiceClient(connectionString);
        }


        private static byte[] ReadFileHead(IFormFile file)
        {
            using var fs = new BinaryReader(file.OpenReadStream());
            var bytes = new byte[20];
            fs.Read(bytes, 0, 20);
            return bytes;
        }

        private static bool ValidateMaliciousFile(IFormFile file)
        {
            var sniffer = new Sniffer();

            sniffer.Populate(FileTypes.CommonFileTypes);

            using var fs = new BinaryReader(file.OpenReadStream());
            var bytes = new byte[20];
            fs.Read(bytes, 0, 20);

            var result = sniffer.Match(bytes);

            if (result.Count > 0)
                return true;
            else
                return false;
        }

        public async Task GetFile(string containerName, string fileName)
        {
            try
            {
                //var blobContainerResponse = _blobServiceClient.GetBlobContainersAsync(containerName);

                //blobContainerResponse.

            }
            catch (Exception ex)
            {

            }
        }

        public async Task UploadFile(string containerName, string fileName, IFormFile file)
        {
            try
            {
                var blobContainerResponse = await _blobServiceClient.CreateBlobContainerAsync(containerName);

                var blobContainer = blobContainerResponse.Value;

                var blob = blobContainer.GetBlobClient(fileName);

                using var fileData = file.OpenReadStream();

                var result = await blob.UploadAsync(fileData);

                //blob.Uri
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }


        public async Task TestingMethod()
        {
            BlobContainerClient containerClient = 
                await _blobServiceClient.CreateBlobContainerAsync("TestingContainer" + Guid.NewGuid().ToString());
            
            var blobClient = containerClient.GetBlobClient("TestingName" + Guid.NewGuid().ToString() + ".txt");

            Console.WriteLine(blobClient.Uri);

        }
    }
}
