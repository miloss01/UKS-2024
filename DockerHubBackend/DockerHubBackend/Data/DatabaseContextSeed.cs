using DockerHubBackend.Models;

namespace DockerHubBackend.Data
{
    public static class DatabaseContextSeed
    {
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new StandardUser { Email = "user1@email.com", IsVerified = true, Password = "test123"},
                    new StandardUser { Email = "user2@email.com", IsVerified = true, Password = "123456" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
