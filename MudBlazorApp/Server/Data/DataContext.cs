using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    Description = "create, read, update, delete",
                    CreatedDate = DateTime.Now
                },
                new Role
                {
                    Id = 2,
                    Name = "Customer",
                    Description = "read",
                    CreatedDate = DateTime.Now
                });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.ChatMessagesFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ChatMessagesToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
