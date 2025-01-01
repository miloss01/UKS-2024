using DockerHubBackend.Dto.Request;
using DockerHubBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace DockerHubBackend.Tests.IntegrationTests
{
    public class ExampleTest : IntegrationTestBase
    {
        [Fact]
        public async Task Example_Test()
        {
            using var dbContext = GetDbContext();

            var user = new StandardUser { Email = "user@email.com", Password = "pass", Username = "user", Id = Guid.NewGuid() };
            var repo = new DockerRepository { Id = Guid.NewGuid(), Name = "repo", IsPublic = true, UserOwner = user, UserOwnerId = user.Id };
            var img = new DockerImage { Id = Guid.NewGuid(), DockerRepositoryId = repo.Id, Repository = repo };

            dbContext.Users.Add(user);
            //dbContext.SaveChanges();
            dbContext.DockerRepositories.Add(repo);
            dbContext.DockerImages.Add(img);

            //dbContext.SaveChanges();

            var response = await _httpClient.GetAsync("/api/dockerImages?page=1&pageSize=10");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Console.WriteLine(responseString);
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        }
    }
}
