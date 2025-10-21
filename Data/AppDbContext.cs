using Microsoft.EntityFrameworkCore;
using video_processing_api.Models;

namespace video_processing_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Video>()
                .Property(v => v.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Video>()
                .Property(v => v.Status)
                .HasMaxLength(50);
        }
    }
}