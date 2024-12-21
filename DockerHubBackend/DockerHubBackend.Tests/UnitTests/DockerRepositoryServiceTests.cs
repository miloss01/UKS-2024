using DockerHubBackend.Dto.Response;
using DockerHubBackend.Exceptions;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Services.Implementation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerHubBackend.Tests.UnitTests
{
    public class DockerRepositoryServiceTests
    {
        private readonly Mock<IDockerRepositoryRepository> _mockDockerRepositoryRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly DockerRepositoryService _service;

        public DockerRepositoryServiceTests()
        {
            _mockDockerRepositoryRepository = new Mock<IDockerRepositoryRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
			_service = new DockerRepositoryService(_mockDockerRepositoryRepository.Object, _mockUserRepository.Object);
        }

        [Fact]
        public void GetDockerRepositoryById_ProvideIdThatExists_ReturnsProvidedDockerRepository()
        {
            var dockerRepository = new DockerRepository { Id = Guid.NewGuid(), Name = "repo1" };

            _mockDockerRepositoryRepository.Setup(dockerRepositoryRepository => dockerRepositoryRepository.GetFullDockerRepositoryById(dockerRepository.Id)).Returns(dockerRepository);

            var result = _service.GetDockerRepositoryById(dockerRepository.Id);

            Assert.Equal(dockerRepository, result);
        }

        [Fact]
        public void GetDockerRepositoryById_ProvideIdThatDoesNotExist_ThrowsNotFoundException()
        {
            DockerRepository dockerRepository = null;
            Guid id = Guid.NewGuid();

            _mockDockerRepositoryRepository.Setup(dockerRepositoryRepository => dockerRepositoryRepository.GetFullDockerRepositoryById(It.IsAny<Guid>())).Returns(dockerRepository);

            var exception = Assert.Throws<NotFoundException>(() => _service.GetDockerRepositoryById(id));

            Assert.Equal($"Docker repository with id {id.ToString()} not found.", exception.Message);
        }
    }
}
