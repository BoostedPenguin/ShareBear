using Microsoft.EntityFrameworkCore;
using ShareBear.Data;

namespace ShareBear.Services
{
    public class ScheduledFileDeletion
    {
        // Run every 15 minutes
        const double ScheduledDeletionTime = 15 * 60 * 1000;

        private System.Timers.Timer? timer = null;
        private readonly IDbContextFactory<DefaultContext> contextFactory;
        private readonly ILogger<ScheduledFileDeletion> logger;

        public ScheduledFileDeletion(IDbContextFactory<DefaultContext> contextFactory, ILogger<ScheduledFileDeletion> logger)
        {
            this.contextFactory = contextFactory;
            this.logger = logger;
        }

        public void InitScheduledDeletion()
        {
            timer = new System.Timers.Timer()
            {
                // Run every 15 minutes
                Interval = ScheduledDeletionTime,
                AutoReset = true,
                Enabled = true,
            };
            timer.Elapsed += DeleteExpiredContainers;


            DeleteExpiredContainers();
        }




        private void DeleteExpiredContainers(object? sender, System.Timers.ElapsedEventArgs e)
        {
            DeleteExpiredContainers();
        }

        private async void DeleteExpiredContainers()
        {
            try
            {
                logger.LogInformation("Scheduled deletion commencing...");
                using var db = contextFactory.CreateDbContext();

                var newlyExpiredContainers = await db.ContainerHubs.Where(e => e.ExpiresAt <= DateTime.UtcNow && e.IsActive).ToListAsync();

                if (newlyExpiredContainers.Count == 0)
                {
                    logger.LogInformation($"Scheduled deletion completed. No entries. Next deletion in {ScheduledDeletionTime / 60 / 1000} minutes.");
                    return;
                }

                newlyExpiredContainers.ForEach(e => e.IsActive = false);


                await db.SaveChangesAsync();
                logger.LogInformation($"Scheduled deletion completed. Deleted entries: {newlyExpiredContainers.Count}. Next deletion in {ScheduledDeletionTime / 60 / 1000} minutes.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Scheduled deletion failed");
                logger.LogError(ex.Message);
            }
        }
    }
}
