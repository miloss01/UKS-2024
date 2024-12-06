using DockerHubBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DockerHubBackend.Data
{

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<BaseUser> Users { get; set; }
        public DbSet<DockerImage> DockerImages { get; set; }
        public DbSet<DockerRepository> DockerRepositories { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamPermission> TeamPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseUser>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Admin>("Admin")
                .HasValue<SuperAdmin>("SuperAdmin")
                .HasValue<StandardUser>("StandardUser");


            modelBuilder.Entity<DockerImage>()
                .Property(p => p.Tags)
                .HasConversion(v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
            v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null));

            base.OnModelCreating(modelBuilder);
        }
    }
}
