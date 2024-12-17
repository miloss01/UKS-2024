using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Exceptions;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Security;
using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace DockerHubBackend.Services.Implementation
{
    public class DockerRepositoryService : IDockerRepositoryService
    {
        private readonly IDockerRepositoryRepository _dockerRepositoryRepository;

        public DockerRepositoryService(IDockerRepositoryRepository dockerRepositoryRepository)
        {
            _dockerRepositoryRepository = dockerRepositoryRepository;
        }

        public DockerRepository GetDockerRepositoryById(Guid id)
        {
            var dockerRepository = _dockerRepositoryRepository.GetFullDockerRepositoryById(id);

            if (dockerRepository == null)
            {
                throw new NotFoundException($"Docker repository with id {id.ToString()} not found.");
            }

            return dockerRepository;
        }
    }
}