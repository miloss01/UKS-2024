using DockerHubBackend.Models;
using Microsoft.AspNetCore.Identity;

namespace DockerHubBackend.Data
{
    public static class DatabaseContextSeed
    {
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<string>>();

            var passwordHash = passwordHasher.HashPassword(String.Empty, "123456");
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new StandardUser { Email = "user1@email.com", Password = passwordHash },
                    new StandardUser { Email = "user2@email.com", Password = passwordHash },
                    new SuperAdmin { Email = "super.admin@email.com", Password = passwordHash, IsVerified = false}
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
