using Microsoft.EntityFrameworkCore;
using ShareBear.Data;
using ShareBear.Data.Models;

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
        Task GenerateContainer(string visitorId, List<IFormFile> files);
    }

    public class FileAccessService : IFileAccessService
    {
        // In MB
        const int MaxStorageSize = 1000;
        private readonly IDbContextFactory<DefaultContext> contextFactory;
        private readonly IAzureStorageService azureStorageService;
        private readonly ILogger<FileAccessService> logger;
        private readonly IWebHostEnvironment env;

        public FileAccessService(IDbContextFactory<DefaultContext> contextFactory, IAzureStorageService azureStorageService, ILogger<FileAccessService> logger, IWebHostEnvironment env)
        {
            this.contextFactory = contextFactory;
            this.azureStorageService = azureStorageService;
            this.logger = logger;
            this.env = env;
        }

        /**
         * The entry point of creating a bucket
         */
        public async Task GenerateContainer(string visitorId, List<IFormFile> files)
        {
            var size = await azureStorageService.GetTotalSizeContainers();

            if (files.Count == 0)
                throw new ArgumentException("You did not send any files.");

            if (size.Value.MegaBytes >= MaxStorageSize)
                throw new OutOfStorageException($"The storage is currently at max capacity. Check back later.");


            using var db = contextFactory.CreateDbContext();

            var visitorActiveContainerCount =
                await db.ContainerHubs.Where(e => e.IsActive && e.CreatedByVisitorId == visitorId).CountAsync();

            if (visitorActiveContainerCount > 3)
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
        }
    }
}
