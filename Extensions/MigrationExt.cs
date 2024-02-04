using Microsoft.EntityFrameworkCore;
using UrlShorter;

namespace UrlShorter.Extensions
{
    public static class MigrationExt
    {
        public static void ApplyMigration(this WebApplication app) { 
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
