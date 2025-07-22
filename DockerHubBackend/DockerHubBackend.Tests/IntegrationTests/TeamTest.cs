using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response.Organization;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Implementation;
using DockerHubBackend.Repository.Interface;
using Nest;
using DockerHubBackend.Dto.Response;

namespace DockerHubBackend.Tests.IntegrationTests
{
    public class TeamTest : IntegrationTestBase
    {

        [Fact]
        public async Task CreateTeam_ShouldCreateTeam()
        {
            using var dbContext = GetDbContext();

            var user = new StandardUser { Email = "user@email.com", Password = "pass", Username = "user", Id = Guid.NewGuid() };
            Organization org = new Organization
            {
                Name = "Code org",
                Description = "This is some organization. This is an example of org description.",
                ImageLocation = "some_loc",
                OwnerId = user.Id,
                Owner = user,
            };
            dbContext.Users.Add(user);
            dbContext.Organizations.Add(org);
            dbContext.SaveChanges();
            dbContext.ChangeTracker.Clear();

            MemberDto memberDto = new MemberDto();
            memberDto.Email = user.Email;
            TeamDto teamDto = new TeamDto("Teamteam", "This is some description.", [memberDto], org.Id);
            
            var response = await _httpClient.PostAsJsonAsync("/api/team", teamDto);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadFromJsonAsync<TeamDto>();
            Assert.NotNull(content);
            Assert.Equal("Teamteam", content.Name);
            Assert.Single(content.Members);
            Assert.Equal(user.Email, content.Members.First().Email);
        }

        [Fact]
        public async Task CreateTeam_DuplicateName_ShouldReturnBadRequest()
        {
            using var dbContext = GetDbContext();

            var user = new StandardUser { Email = "user@email.com", Password = "pass", Username = "user", Id = Guid.NewGuid() };
            Organization org = new Organization
            {
                Name = "Code org",
                Description = "This is some organization. This is an example of org description.",
                ImageLocation = "some_loc",
                OwnerId = user.Id,
                Owner = user,
            };
            Team team = new Team { Name = "Teamteam", Organization = org, OrganizationId = org.Id };

            dbContext.Users.Add(user);
            dbContext.Organizations.Add(org);
            dbContext.Teams.Add(team);
            dbContext.SaveChanges();
            dbContext.ChangeTracker.Clear();

            var duplicateTeamDto = new
            {
                Name = "Teamteam",
                Description = "Clashing name",
                OrganizationId = org.Id,
                Members = new[] { new { Email = user.Email } }
            };

            var response = await _httpClient.PostAsJsonAsync("/api/team", duplicateTeamDto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AddMembers_ValidUser_ShouldAddToTeam()
        {
            using var dbContext = GetDbContext();

            var user = new StandardUser { Email = "user1@email.com", Password = "pass", Username = "user", Id = Guid.NewGuid() };
            var user1 = new StandardUser { Email = "user2@email.com", Password = "pass", Username = "useruser", Id = Guid.NewGuid() };
            Organization org = new Organization
            {
                Name = "Code org",
                Description = "This is some organization. This is an example of org description.",
                ImageLocation = "some_loc",
                OwnerId = user.Id,
                Owner = user,
                Id=Guid.NewGuid(),
            };
            var team = new Team { Id = Guid.NewGuid(), Name = "Team1", Organization = org, OrganizationId = org.Id, Members = new HashSet<StandardUser>() };

            dbContext.Organizations.Add(org);
            Console.WriteLine(user.Id);
            Console.WriteLine(user1.Id);
            dbContext.Users.Add(user);
            dbContext.Users.Add(user1);
            dbContext.Teams.Add(team);
            dbContext.SaveChanges();

            List<MemberDto> membersDto = [new MemberDto { Email = "user2@email.com", Id = Guid.NewGuid() }];

            var response = await _httpClient.PutAsJsonAsync($"/api/team/member/{team.Id}", membersDto);
            var errorContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<TeamDto>();
            Assert.NotNull(content);
            Assert.Equal(team.Id, content.Id);
            Assert.Single(content.Members);
            Assert.Equal(user1.Email, content.Members.First().Email);
        }

        [Fact]
        public async Task AddPermissions_ValidInput_ShouldAddPermission()
        {
            using var dbContext = GetDbContext();
            var user = new StandardUser { Email = "user1@email.com", Password = "pass", Username = "user", Id = Guid.NewGuid() };
            Organization org = new Organization
            {
                Name = "Code org",
                Description = "This is some organization. This is an example of org description.",
                ImageLocation = "some_loc",
                OwnerId = user.Id,
                Owner = user,
                Id = Guid.NewGuid(),
            };
            var team = new Team { Id = Guid.NewGuid(), Name = "Team1", Organization = org, OrganizationId = org.Id, Members = new HashSet<StandardUser>() };
            DockerRepository repo = new DockerRepository { Name = "Some repository" };

            dbContext.Organizations.Add(org);
            dbContext.Users.Add(user);
            dbContext.DockerRepositories.Add(repo);
            dbContext.Teams.Add(team);
            dbContext.SaveChanges();

            TeamPermissionRequestDto permissionDto = new TeamPermissionRequestDto
            {
                TeamId = team.Id,
                RepositoryId = repo.Id,
                Permission = "ReadOnly"
            };

            var response = await _httpClient.PostAsJsonAsync("/api/team/permission", permissionDto);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            TeamPermissionResponseDto? content = await response.Content.ReadFromJsonAsync<TeamPermissionResponseDto>();
            Assert.NotNull(content);
            Assert.Equal(team.Name, content.TeamName);
            Assert.Equal(repo.Name, content.RepositoryName);
            Assert.Equal("ReadOnly", content.Permission);
        }

    }
}
