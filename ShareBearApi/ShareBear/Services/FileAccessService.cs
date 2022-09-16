using Microsoft.EntityFrameworkCore;
using ShareBear.Data;
using ShareBear.Data.Models;

namespace ShareBear.Services
{
    public class FileAccessService
    {
        private readonly IDbContextFactory<DefaultContext> contextFactory;
        private readonly AzureStorageService azureStorageService;
        private readonly ILogger<FileAccessService> logger;
        private readonly IWebHostEnvironment env;

        public FileAccessService(IDbContextFactory<DefaultContext> contextFactory, AzureStorageService azureStorageService, ILogger<FileAccessService> logger, IWebHostEnvironment env)
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
            using var db = contextFactory.CreateDbContext();

            var visitorActiveContainerCount = 
                await db.ContainerHubs.Where(e => e.IsActive && e.CreatedByVisitorId == visitorId).CountAsync();

            if (visitorActiveContainerCount > 3)
                throw new ArgumentException("The max containers per visitor is 3. You have reached that limit. Delete older containers.");

            var visitorContainer = new ContainerHubs(visitorId, env.IsProduction());

            // Add first action as uploaded files
            visitorContainer.ContainerHubAccessLogs.Add(new ContainerHubAccessLogs(visitorId, ContainerUserActions.UPLOADED_FILES));

            await azureStorageService.CreateContainer(visitorContainer.ContainerName);

        }
    }
}
