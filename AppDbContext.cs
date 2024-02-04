using Microsoft.EntityFrameworkCore;
using UrlShorter.Entittes;
using UrlShorter.Services;

namespace UrlShorter
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options){
            
        }

        public DbSet<ShortenUrl> ShortenUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<ShortenUrl>(builder =>
            {
                builder.Property(s => s.Code).HasMaxLength(UrlServices.getLenghtOfShort());

                builder.HasIndex(s=> s.Code).IsUnique();
            });
        }

    }
}
