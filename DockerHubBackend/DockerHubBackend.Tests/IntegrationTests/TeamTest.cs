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
using Amazon.S3.Model;

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
        public async Task CreateTeam_NonExistingOrganization_ShouldReturnNotFound()
        {
            using var dbContext = GetDbContext();
            var user = new StandardUser { Email = "user@email.com", Password = "pass", Username = "user", Id = Guid.NewGuid() };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            dbContext.ChangeTracker.Clear();

            var dto = new
            {
                Name = "Teamteam",
                Description = "Clashing name",
                OrganizationId = Guid.NewGuid(),
                Members = new[] { new { user.Email } }
            };

            var response = await _httpClient.PostAsJsonAsync("/api/team", dto);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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
        public async Task AddMembers_TeamDoesNotExist_ShouldReturnNotFound()
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
                Id = Guid.NewGuid(),
            };

            dbContext.Organizations.Add(org);
            Console.WriteLine(user.Id);
            Console.WriteLine(user1.Id);
            dbContext.Users.Add(user);
            dbContext.Users.Add(user1);
            dbContext.SaveChanges();

            List<MemberDto> membersDto = [new MemberDto { Email = "user2@email.com", Id = Guid.NewGuid() }];

            var response = await _httpClient.PutAsJsonAsync($"/api/team/member/{Guid.NewGuid()}", membersDto);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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
                TeamId = team.Id.ToString(),
                RepositoryId = repo.Id.ToString(),
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

        [Fact]
        public async Task AddPermission_AlreadyExists_ShouldReturnBadRequest()
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
            DockerRepository repo = new DockerRepository { Name = "Some repository" };
            var team = new Team { Id = Guid.NewGuid(), Name = "Team1", Organization = org, OrganizationId = org.Id, Members = new HashSet<StandardUser>() };
            var permission = new TeamPermission { TeamId = team.Id, RepositoryId = repo.Id, Team = team, Repository = repo, Permission = PermissionType.ReadOnly };

            dbContext.Users.Add(user);
            dbContext.Organizations.Add(org);
            dbContext.DockerRepositories.Add(repo);
            dbContext.Teams.Add(team);
            dbContext.TeamPermissions.Add(permission);
            dbContext.SaveChanges();

            var request = new
            {
                TeamId = team.Id,
                RepositoryId = repo.Id,
                Permission = "ReadOnly"
            };

            var response = await _httpClient.PostAsJsonAsync("/api/team/permission", request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var error = await response.Content.ReadAsStringAsync();
            Assert.Contains("Team-Permission already exists", error);
        }

        [Fact]
        public async Task AddPermission_TeamNotFound_ShouldReturnNotFound()
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
            DockerRepository repo = new DockerRepository { Name = "Some repository" };

            dbContext.Users.Add(user);
            dbContext.Organizations.Add(org);
            dbContext.DockerRepositories.Add(repo);
            dbContext.SaveChanges();

            var request = new
            {
                TeamId = Guid.NewGuid(),
                RepositoryId = repo.Id,
                Permission = "ReadOnly"
            };

            var response = await _httpClient.PostAsJsonAsync("/api/team/permission", request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var error = await response.Content.ReadAsStringAsync();
            Assert.Contains("Team not found", error);
        }

        [Fact]
        public async Task AddPermission_RepositoryNotFound_ShouldReturnNotFound()
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

            dbContext.Users.Add(user);
            dbContext.Organizations.Add(org);
            dbContext.Teams.Add(team);
            dbContext.DockerRepositories.Add(repo);
            dbContext.SaveChanges();

            var request = new
            {
                TeamId = team.Id,
                RepositoryId = Guid.NewGuid(),
                Permission = "ReadOnly"
            };

            var response = await _httpClient.PostAsJsonAsync("/api/team/permission", request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var error = await response.Content.ReadAsStringAsync();
            Assert.Contains("Repository not found.", error);
        }

        [Fact]
        public async Task GetTeamsByOrganizationId_ShouldReturnTeams()
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
            var team2 = new Team { Id = Guid.NewGuid(), Name = "Team2", Organization = org, OrganizationId = org.Id, Members = new HashSet<StandardUser>() };

            dbContext.Users.Add(user);
            dbContext.Organizations.Add(org);
            dbContext.Teams.AddRange(team, team2);
            dbContext.SaveChanges();

            var response = await _httpClient.GetAsync($"/api/team/org/{org.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var teams = await response.Content.ReadFromJsonAsync<List<TeamDto>>();
            Assert.NotNull(teams);
            Assert.Equal(2, teams.Count);
        }

        [Fact]
        public async Task GetTeamById_ShouldReturnCorrectTeam()
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

            dbContext.Organizations.Add(org);
            dbContext.Teams.Add(team);
            dbContext.SaveChanges();

            var response = await _httpClient.GetAsync($"/api/team/{team.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var dto = await response.Content.ReadFromJsonAsync<TeamDto>();
            Assert.NotNull(dto);
            Assert.Equal("Team1", dto.Name);
        }

        [Fact]
        public async Task GetTeam_InvalidId_ShouldReturnNotFound()
        {
            var invalidId = Guid.NewGuid();
            var response = await _httpClient.GetAsync($"/api/team/{invalidId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var error = await response.Content.ReadAsStringAsync();
            Assert.Contains("Team not found", error);
        }


        [Fact]
        public async Task UpdateTeam_ValidData_ShouldUpdateSuccessfully()
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

            dbContext.Organizations.Add(org);
            dbContext.Teams.Add(team);
            dbContext.SaveChanges();
            dbContext.ChangeTracker.Clear();

            var updateDto = new TeamDto { Id = Guid.NewGuid(), Name = "Team123", Description="After", OrganizationId = org.Id, Members = new HashSet<MemberDto>() };

            var response = await _httpClient.PutAsJsonAsync($"/api/team/{team.Id}", updateDto);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<TeamDto>();
            Assert.NotNull(result);
            Assert.Equal("Team123", result.Name);
            Assert.Equal("After", result.Description);
        }

        [Fact]
        public async Task UpdateTeam_TeamNotFound_ReturnNotFound()
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

            dbContext.Organizations.Add(org);
            dbContext.Teams.Add(team);
            dbContext.SaveChanges();
            dbContext.ChangeTracker.Clear();

            var updateDto = new TeamDto { Id = Guid.NewGuid(), Name = "Team12345", Description = "After", OrganizationId = org.Id, Members = new HashSet<MemberDto>() };

            var response = await _httpClient.PutAsJsonAsync($"/api/team/{Guid.NewGuid()}", updateDto);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        public async Task DeleteTeam_ExistingId_ShouldDeleteSuccessfully()
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

            dbContext.Organizations.Add(org);
            dbContext.Teams.Add(team);
            dbContext.SaveChanges();

            var response = await _httpClient.DeleteAsync($"/api/team/{team.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var deleted = await response.Content.ReadFromJsonAsync<TeamDto>();
            Assert.Equal("Team1", deleted.Name);
        }

        [Fact]
        public async Task GetPermissions_ShouldReturnPermissions()
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
            var permission = new TeamPermission { TeamId = team.Id, RepositoryId = repo.Id, Team = team, Repository = repo, Permission = PermissionType.Admin };

            dbContext.Users.Add(user);
            dbContext.Organizations.Add(org);
            dbContext.DockerRepositories.Add(repo);
            dbContext.Teams.Add(team);
            dbContext.TeamPermissions.Add(permission);
            dbContext.SaveChanges();

            var response = await _httpClient.GetAsync($"/api/team/repositories/{team.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var permissions = await response.Content.ReadFromJsonAsync<List<TeamPermission>>();
            Assert.NotNull(permissions);
            Assert.Single(permissions);
            Assert.Equal(PermissionType.Admin, permissions[0].Permission);
        }
    }
}
