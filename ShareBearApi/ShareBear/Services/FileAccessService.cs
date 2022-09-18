using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShareBear.Data;
using ShareBear.Data.Models;
using ShareBear.Dtos;

namespace ShareBear.Services
{
    public class OutOfStorageException: Exception
    {
        public OutOfStorageException(string msg ) : base( msg )
        {

        }
    }

    public interface IFileAccessService
    {
        Task<ContainerHubsDto> GenerateContainer(string visitorId, List<IFormFile> files);
        Task<ContainerHubsDto> GetContainerFiles(string visitorId, string shortRequestCode);
    }

    public class FileAccessService : IFileAccessService
    {
        // In MB
        const int MaxStorageSize = 1000;
        const int MaxActiveContainersPerVisitor = 3;
        private readonly IDbContextFactory<DefaultContext> contextFactory;
        private readonly IAzureStorageService azureStorageService;
        private readonly ILogger<FileAccessService> logger;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment env;

        public FileAccessService(
            IDbContextFactory<DefaultContext> contextFactory, 
            IAzureStorageService azureStorageService, 
            ILogger<FileAccessService> logger, 
            IMapper mapper,
            IWebHostEnvironment env)
        {
            this.contextFactory = contextFactory;
            this.azureStorageService = azureStorageService;
            this.logger = logger;
            this.mapper = mapper;
            this.env = env;
        }

        /**
         * The entry point of creating a bucket
         */
        public async Task<ContainerHubsDto> GenerateContainer(string visitorId, List<IFormFile> files)
        {
            var size = await azureStorageService.GetTotalSizeContainers();

            if (files.Count == 0)
                throw new ArgumentException("You did not send any files.");

            if (size.Value.MegaBytes >= MaxStorageSize)
                throw new OutOfStorageException($"The storage is currently at max capacity. Check back later.");


            using var db = contextFactory.CreateDbContext();

            var visitorActiveContainerCount =
                await db.ContainerHubs.Where(e => e.IsActive && e.CreatedByVisitorId == visitorId).CountAsync();

            if (visitorActiveContainerCount > MaxActiveContainersPerVisitor)
                throw new ArgumentException("The max containers per visitor is 3. You have reached that limit. Delete older containers.");

            var visitorContainer = new ContainerHubs(visitorId, env.IsProduction());

            // Add first action as uploaded files
            visitorContainer.ContainerHubAccessLogs.Add(new ContainerHubAccessLogs(visitorId, ContainerUserActions.UPLOADED_FILES));

            var containerFiles = files.Select(e => new ContainerFiles(e, env.IsProduction())).ToList();

            await azureStorageService.CreateContainer(visitorContainer.ContainerName);

            await azureStorageService.UploadFiles(visitorContainer.ContainerName, containerFiles.Select(e => new FormFileFileNames()
            {
                File = e.File,
                FileName = e.FileName
            }).ToList());

            containerFiles.ForEach(e => visitorContainer.ContainerFiles.Add(e));
            await db.AddAsync(visitorContainer);
            await db.SaveChangesAsync();

            return await GetContainerFiles(visitorContainer);

        }


        public async Task<ContainerHubsDto> GetContainerFiles(string visitorId, string shortRequestCode)
        {
            using var db = contextFactory.CreateDbContext();

            var container = 
                await db.ContainerHubs
                .Include(e => e.ContainerFiles)
                .FirstOrDefaultAsync(e => e.ShortCodeString == shortRequestCode);

            return await GetContainerFiles(container);
        }
        
        private async Task<ContainerHubsDto> GetContainerFiles(ContainerHubs? container)
        {
            if (container == null)
                throw new DirectoryNotFoundException("This container does not exist.");

            if (container.ContainerFiles == null)
            {
                return mapper.Map<ContainerHubsDto>(container);
            }

            if (container.ExpiresAt <= DateTime.UtcNow)
                throw new ArgumentException("This container has expired");


            var signedLinks = await azureStorageService.GetSignedContainerDownloadLinks(container.ContainerName);

            var result = mapper.Map<ContainerHubsDto>(container);

            foreach (var file in result.ContainerFiles)
            {
                file.SignedItemUrl = signedLinks.First(e => e.FileName == file.FileName).SignedItemUrl;
            }

            return result;
        }
    }
}
