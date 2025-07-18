using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
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
        private readonly Mock<ITeamRepository> _teamRepository;
        private readonly Mock<IOrganizationRepository> _organizationRepository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IDockerRepositoryRepository> _dockerRepositoryRepository;
        private readonly Mock<ILogger<TeamService>> _logger;

        public TeamServiceTests()
        {
            _teamRepository = new Mock<ITeamRepository>();
            _organizationRepository = new Mock<IOrganizationRepository>();
            _userRepository = new Mock<IUserRepository>();
            _dockerRepositoryRepository = new Mock<IDockerRepositoryRepository>();
            _logger = new Mock<ILogger<TeamService>>();

            _teamService = new TeamService(
                _teamRepository.Object,
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

            _teamRepository.Setup(r => r.GetByOrganizationId(orgId)).ReturnsAsync(teams);

            var result = await _teamService.GetTeams(orgId);

            Assert.Single(result);
            Assert.Equal("Alpha", result.First().Name);
        }

        [Fact]
        public async Task Get_ValidTeamId_ReturnsTeamDto()
        {
            var teamId = Guid.NewGuid();
            var team = new Team { Id = teamId, Name = "Bravo", Description = "Test",
                OrganizationId = Guid.NewGuid(),
                Organization = new Organization { Name = "Org", ImageLocation = "img.jpg", OwnerId = Guid.NewGuid(), Owner = new StandardUser
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

            _teamRepository.Setup(r => r.Get(teamId)).ReturnsAsync(team);

            var result = await _teamService.Get(teamId);

            Assert.Equal("Bravo", result.Name);
        }

        [Fact]
        public async Task Get_InvalidTeamId_ThrowsNotFoundException()
        {
            _teamRepository.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Team?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _teamService.Get(Guid.NewGuid()));
        }

        [Fact]
        public async Task Create_ValidDto_ReturnsCreatedTeam()
        {
            var orgId = Guid.NewGuid();
            var dto = new TeamDto { Name = "Charlie", OrganizationId = orgId, Members = new List<EmailDto>() };
            var org = new Organization { Id = orgId, Name = "Org", ImageLocation = "img.jpg", OwnerId = Guid.NewGuid(), Owner = new StandardUser
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
            _teamRepository.Setup(r => r.GetByOrgIdAndTeamName(orgId, dto.Name)).ReturnsAsync((Team?)null);
            _teamRepository
                .Setup(r => r.Create(It.IsAny<Team>()))
                .Returns((Team t) => Task.FromResult(t));
            _teamRepository.Setup(r => r.GetByName(dto.Name)).ReturnsAsync(createdTeam);

            var result = await _teamService.Create(dto);

            Assert.Equal("Charlie", result.Name);
        }

        [Fact]
        public async Task Create_DuplicateTeam_ThrowsBadRequest()
        {
            var orgId = Guid.NewGuid();
            var dto = new TeamDto { Name = "Charlie", OrganizationId = orgId };
            var org = new Organization { Id = orgId, Name = "Org", ImageLocation = "img.jpg", OwnerId = Guid.NewGuid(), Owner = new StandardUser
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
            _teamRepository.Setup(r => r.GetByOrgIdAndTeamName(orgId, dto.Name)).ReturnsAsync(team);

            await Assert.ThrowsAsync<BadRequestException>(() => _teamService.Create(dto));
        }

        [Fact]
        public async Task Delete_ValidTeamId_ReturnsDeletedTeam()
        {
            var teamId = Guid.NewGuid();
            var team = new Team { Id = teamId, Name = "Delta", OrganizationId = Guid.NewGuid(), Organization = new Organization { Name = "Org", ImageLocation = "img.jpg", OwnerId = Guid.NewGuid(), Owner = new StandardUser
            {
                Email = "a@example.com",
                Password = "password123",
                Username = "username",
                Location = "Earth",
                Badge = Badge.NoBadge,
                CreatedAt = DateTime.UtcNow
            },
            } };

            _teamRepository.Setup(r => r.Delete(teamId)).ReturnsAsync(team);

            var result = await _teamService.Delete(teamId);

            Assert.Equal("Delta", result.Name);
        }

        [Fact]
        public async Task Delete_InvalidTeamId_ThrowsNotFound()
        {
            _teamRepository.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync((Team?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => _teamService.Delete(Guid.NewGuid()));
        }
    }
}
