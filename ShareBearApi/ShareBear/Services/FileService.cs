using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Myrmec;
using ShareBear.Helpers;

namespace ShareBear.Services
{
    public interface IFileService
    {
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

        public FileService(IOptions<AppSettings> appSettings, IWebHostEnvironment env)
        {
            this.appSettings = appSettings;
            this.env = env;
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
