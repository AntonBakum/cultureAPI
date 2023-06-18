using FluentMigrator.Runner;


namespace CultureAPI.Infrastructure.DataLayer.Migrations
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication webApplication)
        {
            using (var scope = webApplication.Services.CreateScope())
            {
                IMigrationRunner migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                try
                {
                    migrationService.ListMigrations();
                    migrationService.MigrateUp();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return webApplication;
        }
    }
}
