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
                    new StandardUser { Email = "user1@email.com", IsVerified = true, Password = passwordHash },
                    new StandardUser { Email = "user2@email.com", IsVerified = true, Password = passwordHash }
                );
                await context.SaveChangesAsync();
            }

            if (!context.Organizations.Any())
            {
                context.Organizations.AddRange(
                    new Organization 
                    {  
                        Name = "Code org",
                        Description = "This is some organization. This is an example of org description.", 
                        ImageLocation = "some_loc", 
                        OwnerId = context.Users.OrderBy(u => u.Id).First().Id,
                        Owner = (StandardUser) context.Users.OrderBy(u => u.Id).First(),
                    }    
                );
                await context.SaveChangesAsync();
            }

            if (!context.Teams.Any()) 
            {
                HashSet<StandardUser> members = new HashSet<StandardUser>();
                Console.WriteLine("addeddddd");
                members.Add((StandardUser)context.Users.OrderBy(u => u.Id).Last());
                Console.WriteLine(members.ToString());
                context.Teams.AddRange(
                    new Team 
                    {  
                        Name = "Team 1", Description="Some desc",
                        Members = members,
                        Organization = context.Organizations.OrderBy(o => o.Id).First(),
                        OrganizationId = context.Organizations.OrderBy(o => o.Id).First().Id
                    },
                     new Team
                     {
                         Name = "Team 2",
                         Description = "Some desc",
                         Members = members,
                         Organization = context.Organizations.OrderBy(o => o.Id).First(),
                         OrganizationId = context.Organizations.OrderBy(o => o.Id).First().Id
                     }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
