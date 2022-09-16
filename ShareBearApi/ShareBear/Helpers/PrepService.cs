using Microsoft.EntityFrameworkCore;
using ShareBear.Data;
using ShareBear.Data.Models;
using ShareBear.Services;

namespace ShareBear.Helpers
{
    public static class PrepService
    {
        public static async Task PrepMigration(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var contextFactory = 
                serviceScope.ServiceProvider.GetService<IDbContextFactory<DefaultContext>>();


            if (contextFactory == null)
                throw new Exception("Unable to construct context factory!");

            using var context = contextFactory.CreateDbContext();


            if (!context.Database.IsInMemory())
            {
                ApplyMigrations(context);
            }


            await SeedDatabase(context);


            var scheduledFileDeletion =
                serviceScope.ServiceProvider.GetService<ScheduledFileDeletion>();
            if (scheduledFileDeletion == null)
                throw new Exception("Scheduled file deletion service is not working. Critical error.");

            scheduledFileDeletion.InitScheduledDeletion();
        }

        private static async Task SeedDatabase(DefaultContext defaultContext)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
            {
                var duplicate = 
                    await defaultContext.Users.FirstOrDefaultAsync(e => e.Email == "legendsxchaos@gmail.com");

                if (duplicate != null)
                    return;

                var user = new Users()
                {
                    Email = "legendsxchaos@gmail.com",
                    UserGlobalIdentifier = Guid.NewGuid().ToString(),
                    Role = "admin",
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword("Something"),
                    LastLoggedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                };

                var testContainer = new ContainerHubs("visitorId", false);

                await defaultContext.AddAsync(testContainer);

                await defaultContext.AddAsync(user);
                await defaultContext.SaveChangesAsync();
            }
        }

        private static void ApplyMigrations(DefaultContext defaultContext)
        {
            Console.WriteLine("--> Attempting to apply migrations...");
            defaultContext.Database.Migrate();

            Console.WriteLine("--> Migrations added");
        }
    }
}
