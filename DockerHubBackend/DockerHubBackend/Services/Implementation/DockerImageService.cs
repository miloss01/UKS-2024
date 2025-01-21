using DockerHubBackend.Data;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Exceptions;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Services.Interface;

namespace DockerHubBackend.Services.Implementation
{
    public class DockerImageService : IDockerImageService
    {
        private readonly IDockerImageRepository _dockerImageRepository;
        private readonly ILogger<DockerImageService> _logger;
        private readonly IRegistryService _registryService;
        private readonly DataContext _context;

        public DockerImageService(IDockerImageRepository dockerImageRepository, ILogger<DockerImageService> logger, IRegistryService registryService, DataContext context)
        {
            _dockerImageRepository = dockerImageRepository;
            _logger = logger;
            _registryService = registryService;
            _context = context;
        }

        public PageDTO<DockerImage> GetDockerImages(int page, int pageSize, string? searchTerm, string? badges)
        {
            _logger.LogInformation("Fetching Docker images with parameters: Page={Page}, PageSize={PageSize}, SearchTerm={SearchTerm}, Badges={Badges}", page, pageSize, searchTerm, badges);
            var result = _dockerImageRepository.GetDockerImages(page, pageSize, searchTerm, badges);

            _logger.LogInformation("Fetched Docker image.");
            return result;
        }

        private async Task<DockerImage> getImageWithRepository(Guid id)
        {
            _logger.LogInformation("Fetching Docker image with ID: {Id}", id);

            var repository = await _dockerImageRepository.GetDockerImageByIdWithRepository(id);
            if (repository == null)
            {
                _logger.LogError("Docker image with ID: {Id} not found.", id);
                throw new NotFoundException($"Docker image with id {id.ToString()} not found.");
            }

            _logger.LogInformation("Docker image with ID: {Id} found.", id);
            return repository;
        }


        public async Task DeleteDockerImage(Guid id)
        {
            _logger.LogInformation("Attempting to delete Docker image with ID: {Id}", id);

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    DockerImage image = await getImageWithRepository(id);

                    await _dockerImageRepository.Delete(id);
                    await _registryService.DeleteDockerImage(image.Digest, image.Repository.Name);

                    await transaction.CommitAsync();

                    _logger.LogInformation("Successfully deleted Docker image with ID: {Id}", id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Failed to delete Docker image with ID: {Id}", id);
                    throw new Exception("Something went wrong");
                }
            }
        }
    }
}