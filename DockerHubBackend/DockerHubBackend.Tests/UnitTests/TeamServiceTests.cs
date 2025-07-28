using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Dto.Response.Organization;
using DockerHubBackend.Exceptions;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Services.Implementation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DockerHubBackend.Tests.UnitTests
{
    public class TeamServiceTests
    {
        private readonly TeamService _teamService;
        private readonly Mock<ITeamRepository> _repository;
        private readonly Mock<IOrganizationRepository> _organizationRepository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IDockerRepositoryRepository> _dockerRepositoryRepository;
        private readonly Mock<ILogger<TeamService>> _logger;

        public TeamServiceTests()
        {
            _repository = new Mock<ITeamRepository>();
            _organizationRepository = new Mock<IOrganizationRepository>();
            _userRepository = new Mock<IUserRepository>();
            _dockerRepositoryRepository = new Mock<IDockerRepositoryRepository>();
            _logger = new Mock<ILogger<TeamService>>();

            _teamService = new TeamService(
                _repository.Object,
                _organizationRepository.Object,
                _userRepository.Object,
                _dockerRepositoryRepository.Object,
                _logger.Object
            );
        }

        [Fact]
        public async Task GetTeams_ReturnsTeamList()
        {
            var orgId = Guid.NewGuid();
            var teams = new List<TeamDto> { new TeamDto { Name = "Alpha", OrganizationId = orgId } };

            _repository.Setup(r => r.GetByOrganizationId(orgId)).ReturnsAsync(teams);

            var result = await _teamService.GetTeams(orgId);

            Assert.Single(result);
            Assert.Equal("Alpha", result.First().Name);
        }

        [Fact]
        public async Task Get_ValidTeamId_ReturnsTeamDto()
        {
            var teamId = Guid.NewGuid();
            var team = new Team
            {
                Id = teamId,
                Name = "Bravo",
                Description = "Test",
                OrganizationId = Guid.NewGuid(),
                Organization = new Organization
                {
                    Name = "Org",
                    ImageLocation = "img.jpg",
                    OwnerId = Guid.NewGuid(),
                    Owner = new StandardUser
                    {
                        Email = "a@example.com",
                        Password = "password123",
                        Username = "username",
                        Location = "Earth",
                        Badge = Badge.NoBadge,
                        CreatedAt = DateTime.UtcNow
                    },
                }
            };

            _repository.Setup(r => r.Get(teamId)).ReturnsAsync(team);

            var result = await _teamService.Get(teamId);

            Assert.Equal("Bravo", result.Name);
        }

        [Fact]
        public async Task Get_InvalidTeamId_ThrowsNotFoundException()
        {
            _repository.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Team?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _teamService.Get(Guid.NewGuid()));
        }

        [Fact]
        public async Task Create_ValidDto_ReturnsCreatedTeam()
        {
            var orgId = Guid.NewGuid();
            var dto = new TeamDto { Name = "Charlie", OrganizationId = orgId, Members = new List<MemberDto>() };
            var org = new Organization
            {
                Id = orgId,
                Name = "Org",
                ImageLocation = "img.jpg",
                OwnerId = Guid.NewGuid(),
                Owner = new StandardUser
                {
                    Email = "a@example.com",
                    Password = "password123",
                    Username = "username",
                    Location = "Earth",
                    Badge = Badge.NoBadge,
                    CreatedAt = DateTime.UtcNow
                },
            };
            var createdTeam = new Team { Id = Guid.NewGuid(), Name = "Charlie", OrganizationId = orgId, Organization = org };

            _organizationRepository.Setup(r => r.Get(orgId)).ReturnsAsync(org);
            _repository.Setup(r => r.GetByOrgIdAndTeamName(orgId, dto.Name)).ReturnsAsync((Team?)null);
            _repository
                .Setup(r => r.Create(It.IsAny<Team>()))
                .Returns((Team t) => Task.FromResult(t));
            _repository.Setup(r => r.GetByName(dto.Name)).ReturnsAsync(createdTeam);

            var result = await _teamService.Create(dto);

            Assert.Equal("Charlie", result.Name);
        }

        [Fact]
        public async Task Create_DuplicateTeam_ThrowsBadRequest()
        {
            var orgId = Guid.NewGuid();
            var dto = new TeamDto { Name = "Charlie", OrganizationId = orgId };
            var org = new Organization
            {
                Id = orgId,
                Name = "Org",
                ImageLocation = "img.jpg",
                OwnerId = Guid.NewGuid(),
                Owner = new StandardUser
                {
                    Email = "a@example.com",
                    Password = "password123",
                    Username = "username",
                    Location = "Earth",
                    Badge = Badge.NoBadge,
                    CreatedAt = DateTime.UtcNow
                },
            };
            var team = new Team { Name = "Charlie", OrganizationId = orgId, Organization = org };

            _organizationRepository.Setup(r => r.Get(orgId)).ReturnsAsync(org);
            _repository.Setup(r => r.GetByOrgIdAndTeamName(orgId, dto.Name)).ReturnsAsync(team);

            await Assert.ThrowsAsync<BadRequestException>(() => _teamService.Create(dto));
        }

        [Fact]
        public async Task Delete_ValidTeamId_ReturnsDeletedTeam()
        {
            var teamId = Guid.NewGuid();
            var team = new Team
            {
                Id = teamId,
                Name = "Delta",
                OrganizationId = Guid.NewGuid(),
                Organization = new Organization
                {
                    Name = "Org",
                    ImageLocation = "img.jpg",
                    OwnerId = Guid.NewGuid(),
                    Owner = new StandardUser
                    {
                        Email = "a@example.com",
                        Password = "password123",
                        Username = "username",
                        Location = "Earth",
                        Badge = Badge.NoBadge,
                        CreatedAt = DateTime.UtcNow
                    },
                }
            };

            _repository.Setup(r => r.Delete(teamId)).ReturnsAsync(team);

            var result = await _teamService.Delete(teamId);

            Assert.Equal("Delta", result.Name);
        }

        [Fact]
        public async Task Delete_InvalidTeamId_ThrowsNotFound()
        {
            _repository.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync((Team?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _teamService.Delete(Guid.NewGuid()));
        }

        [Fact]
        public async Task AddMembers_ValidTeam_AddsMembers()
        {
            var teamId = Guid.NewGuid();

            var standardUser = new StandardUser
            {
                Email = "a@example.com",
                Username = "user1",
                Password = "pw",
                CreatedAt = DateTime.UtcNow
            };

            var memberDtos = new HashSet<StandardUser>
            {
                standardUser
            };


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
            var team = new Team { Id = Guid.NewGuid(), Name = "Team1", Organization = org, OrganizationId = org.Id, Members = memberDtos };

            _repository.Setup(r => r.Get(teamId)).ReturnsAsync(team);
            _userRepository.Setup(r => r.GetUserByEmail("a@example.com")).ReturnsAsync(standardUser);
            _repository.Setup(r => r.Update(It.IsAny<Team>())).ReturnsAsync((Team t) => t);

            var result = await _teamService.AddMembers(teamId, [new MemberDto { Email = "a@example.com" }]);

            Assert.Single(result.Members);
            Assert.Equal("a@example.com", result.Members.First().Email);
        }

        [Fact]
        public async Task AddMembers_InvalidTeam_ThrowsNotFound()
        {
            _repository.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Team?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _teamService.AddMembers(Guid.NewGuid(), new List<MemberDto>()));
        }

        [Fact]
        public async Task AddPermissions_Valid_AddsSuccessfully()
        {
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

            DockerRepository repo = new DockerRepository { Name = "RepoX" };
            var team = new Team { Id = Guid.NewGuid(), Name = "TeamX", Organization = org, OrganizationId = org.Id, Members = new HashSet<StandardUser>() };
            var dto = new TeamPermissionRequestDto
            {
                TeamId = team.Id.ToString(),
                RepositoryId = repo.Id.ToString(),
                Permission = "ReadOnly"
            };

            _repository.Setup(r => r.GetTeamPermission(repo.Id, team.Id)).Returns((TeamPermission?)null);
            _dockerRepositoryRepository.Setup(r => r.Get(repo.Id)).ReturnsAsync(repo);
            _repository.Setup(r => r.Get(team.Id)).ReturnsAsync(team);
            _repository.Setup(r => r.AddPermissions(It.IsAny<TeamPermission>()));

            TeamPermissionResponseDto result = await _teamService.AddPermissions(dto);

            Assert.Equal("TeamX", result.TeamName);
            Assert.Equal("RepoX", result.RepositoryName);
            Assert.Equal("ReadOnly", result.Permission);
        }

        [Fact]
        public async Task AddPermissions_AlreadyExists_ThrowsBadRequest()
        {
            var dto = new TeamPermissionRequestDto
            {
                TeamId = Guid.NewGuid().ToString(),
                RepositoryId = Guid.NewGuid().ToString(),
                Permission = "Write"
            };

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

            _repository.Setup(r => r.GetTeamPermission(Guid.Parse(dto.RepositoryId), Guid.Parse(dto.TeamId))).Returns(permission);

            await Assert.ThrowsAsync<BadRequestException>(() => _teamService.AddPermissions(dto));
        }

        [Fact]
        public async Task AddPermissions_RepositoryNotFound_ThrowsNotFound()
        {
            var dto = new TeamPermissionRequestDto
            {
                TeamId = Guid.NewGuid().ToString(),
                RepositoryId = Guid.NewGuid().ToString(),
                Permission = "Write"
            };

            _repository.Setup(r => r.GetTeamPermission(Guid.Parse(dto.RepositoryId), Guid.Parse(dto.TeamId))).Returns((TeamPermission?)null);
            _dockerRepositoryRepository.Setup(r => r.Get(Guid.Parse(dto.RepositoryId))).ReturnsAsync((DockerRepository?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _teamService.AddPermissions(dto));
        }

        [Fact]
        public async Task AddPermissions_TeamNotFound_ThrowsNotFound()
        {
            var dto = new TeamPermissionRequestDto
            {
                TeamId = Guid.NewGuid().ToString(),
                RepositoryId = Guid.NewGuid().ToString(),
                Permission = "Write"
            };

            _repository.Setup(r => r.GetTeamPermission(Guid.Parse(dto.RepositoryId), Guid.Parse(dto.TeamId))).Returns((TeamPermission?)null);
            _dockerRepositoryRepository.Setup(r => r.Get(Guid.Parse(dto.RepositoryId))).ReturnsAsync(new DockerRepository { Name = "Repo" });
            _repository.Setup(r => r.Get(Guid.Parse(dto.TeamId))).ReturnsAsync((Team?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _teamService.AddPermissions(dto));
        }

        [Fact]
        public async Task Update_ValidTeam_UpdatesSuccessfully()
        {
            var teamId = Guid.NewGuid();
            var teamDto = new TeamDto { Name = "UpdatedTeam", Description = "Updated" };

            var user = new StandardUser { Email = "user@email.com", Password = "pass", Username = "user", Id = Guid.NewGuid() };
            Organization org = new Organization
            {
                Name = "Code org",
                Description = "This is some organization. This is an example of org description.",
                ImageLocation = "some_loc",
                OwnerId = user.Id,
                Owner = user,
            };
            Team team = new Team { Name = "Teamteam", Organization = org, OrganizationId = org.Id }; ;

            _repository.Setup(r => r.Get(teamId)).ReturnsAsync(team);
            _repository.Setup(r => r.Update(It.IsAny<Team>())).ReturnsAsync((Team t) => t);

            var result = await _teamService.Update(teamDto, teamId);

            Assert.Equal("UpdatedTeam", result.Name);
            Assert.Equal("Updated", result.Description);
        }

        [Fact]
        public async Task Update_InvalidTeam_ThrowsNotFound()
        {
            _repository.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Team?)null);

            var dto = new TeamDto { Name = "DoesNotMatter", Description = "x" };

            await Assert.ThrowsAsync<NotFoundException>(() => _teamService.Update(dto, Guid.NewGuid()));
        }

        [Fact]
        public async Task GetTeamPermissions_ReturnsList()
        {
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
            DockerRepository repo = new DockerRepository { Name = "Some repository", Id = Guid.NewGuid() };

            var perms = new List<TeamPermission>
            {
                new TeamPermission { TeamId = team.Id, Team = team, Permission = PermissionType.ReadOnly, Repository = repo, RepositoryId = repo.Id }
            };

            _repository.Setup(r => r.GetTeamPermissions(team.Id)).ReturnsAsync(perms);

            var result = await _teamService.GetTeamPermissions(team.Id);

            Assert.Single(result);
            Assert.Equal(PermissionType.ReadOnly, result.First().Permission);
        }
    }
}