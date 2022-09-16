using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using ByteSizeLib;
using Microsoft.Extensions.Options;
using Myrmec;
using ShareBear.Helpers;

namespace ShareBear.Services
{
    public interface IAzureStorageService
    {
        Task CreateContainer(string containerName);
        Task DeleteContainer(string containerName);
        Task DeleteFile(string containerName, string fileName);
        Task<ContainerSASItems?> GetSignedContainerDownloadLinks(string containerName);
        Task GetTotalSizeContainers();
        Task UploadFile(string containerName, string fileName, IFormFile file);
        Task UploadFiles(string containerName, List<FormFileFileNames> files);
    }

    public class ContainerSASItems
    {
        public ContainerSASItems(string containerName, string baseUri)
        {
            ContainerName = containerName;
            BaseUri = baseUri;
        }
        public string ContainerName { get; set; }
        public string BaseUri { get; set; }
        public List<string> ContainerItemsUris { get; set; } = new List<string>();
    }

    public struct FormFileFileNames
    {
        public string FileName { get; set; }
        public IFormFile File { get; set; }
    }

    public class InvalidFileTypeException : Exception
    {
        public InvalidFileTypeException(string message) : base(message)
        {

        }
    }

    public class AzureStorageService : IAzureStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IOptions<AppSettings> appSettings;
        private readonly IWebHostEnvironment env;
        private readonly ILogger<AzureStorageService> logger;

        public AzureStorageService(IOptions<AppSettings> appSettings, IWebHostEnvironment env, ILogger<AzureStorageService> logger)
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



        public async Task DeleteContainer(string containerName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                await containerClient.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public async Task DeleteFile(string containerName, string fileName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                if (!await containerClient.ExistsAsync())
                    throw new FileNotFoundException("The given container does not exist.");

                await containerClient.DeleteBlobIfExistsAsync(fileName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public async Task<ContainerSASItems?> GetSignedContainerDownloadLinks(string containerName)
        {
            try
            {

                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                if (!await containerClient.ExistsAsync())
                    throw new FileNotFoundException("The given container does not exist.");

                var containerBlobs = containerClient.GetBlobs();
                var baseUri = containerClient.Uri;


                var storageSharedKeyCredential =
                    new StorageSharedKeyCredential(_blobServiceClient.AccountName, appSettings.Value.AzureStorageAccessKey);

                //  Defines the resource being accessed and for how long the access is allowed.
                var blobSasBuilder = new BlobSasBuilder
                {
                    //StartsOn = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(15d)),
                    ExpiresOn = DateTime.UtcNow.AddDays(15),
                    BlobContainerName = "testingcontainer",
                    
                    // If you omit this, it's going to create an sas for every container item
                    //BlobName = "test.png",
                };

                blobSasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.List);

                var sasQueryParameters = blobSasBuilder.ToSasQueryParameters(storageSharedKeyCredential);



                var result = new ContainerSASItems(containerName, baseUri.ToString());


                var sasUri = new UriBuilder(baseUri)
                {
                    Query = sasQueryParameters.ToString()
                };

                foreach (var item in containerBlobs)
                {
                    sasUri.Path = baseUri.ToString() + "/" + item.Name;
                    result.ContainerItemsUris.Add(sasUri.ToString());
                }

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }


        public async Task UploadFiles(string containerName, List<FormFileFileNames> files)
        {
            var tasks = files.Select(e => UploadFile(containerName, e.FileName, e.File));

            await Task.WhenAll(tasks);
        }

        public async Task CreateContainer(string containerName)
        {
            try
            {
                await _blobServiceClient.CreateBlobContainerAsync(containerName);
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public async Task<ByteSize?> GetTotalSizeContainers()
        {
            try
            {
                string continuationToken = string.Empty;
                var sizes = new Dictionary<string, long>();

                do
                {
                    // Call the listing operation and enumerate the result segment.
                    // When the continuation token is empty, the last segment has been returned
                    // and execution can exit the loop.
                    var resultSegment =
                        _blobServiceClient.GetBlobContainersAsync()
                        .AsPages(continuationToken);

                    await foreach (Azure.Page<BlobContainerItem> containerPage in resultSegment)
                    {

                        foreach (BlobContainerItem containerItem in containerPage.Values)
                        {
                            BlobContainerClient container = new BlobContainerClient(appSettings.Value.AzureStorageConnectionString, containerItem.Name);

                            var size = container.GetBlobs().Sum(b => b.Properties.ContentLength.GetValueOrDefault());

                            sizes.Add(containerItem.Name, size);
                        }

                        // Get the continuation token and loop until it is empty.
                        continuationToken = containerPage.ContinuationToken ?? "";

                        Console.WriteLine();
                    }

                } while (continuationToken != string.Empty);

                return ByteSize.FromBytes(sizes.Sum(e => e.Value));
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }


        public async Task UploadFile(string containerName, string fileName, IFormFile file)
        {
            try
            {
                var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);

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
    }
}
