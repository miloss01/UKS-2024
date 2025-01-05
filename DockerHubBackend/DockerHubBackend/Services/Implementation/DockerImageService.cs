using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Exceptions;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Implementation;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Security;
using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace DockerHubBackend.Services.Implementation
{
    public class DockerImageService : IDockerImageService
    {
        private readonly IDockerImageRepository _dockerImageRepository;
        private readonly ILogger<DockerImageService> _logger;

        public DockerImageService(IDockerImageRepository dockerImageRepository, ILogger<DockerImageService> logger)
        {
            _dockerImageRepository = dockerImageRepository;
            _logger = logger;
        }

        public PageDTO<DockerImage> GetDockerImages(int page, int pageSize, string? searchTerm, string? badges)
        {
            _logger.LogInformation("Fetching Docker images with parameters: Page={Page}, PageSize={PageSize}, SearchTerm={SearchTerm}, Badges={Badges}", page, pageSize, searchTerm, badges);
            var result = _dockerImageRepository.GetDockerImages(page, pageSize, searchTerm, badges);

            _logger.LogInformation("Fetched Docker image.");
            return result;
        }

        private async Task<DockerImage?> getImage(Guid id)
		{
            _logger.LogInformation("Fetching Docker image with ID: {Id}", id);

            var repository = await _dockerImageRepository.GetDockerImageById(id);
			if (repository == null)
			{
                _logger.LogWarning("Docker image with ID: {Id} not found.", id);
                throw new NotFoundException($"Docker image with id {id.ToString()} not found.");
			}

            _logger.LogInformation("Docker image with ID: {Id} found.", id);
            return repository;
		}

		public async Task DeleteDockerImage(Guid id)
		{
            _logger.LogInformation("Attempting to delete Docker image with ID: {Id}", id);
            DockerImage? _ = await getImage(id);

			await _dockerImageRepository.Delete(id);
            _logger.LogInformation("Successfully deleted Docker image with ID: {Id}", id);
        }
    }
}