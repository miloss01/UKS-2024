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

        public DockerImageService(IDockerImageRepository dockerImageRepository)
        {
            _dockerImageRepository = dockerImageRepository;
        }

        public PageDTO<DockerImage> GetDockerImages(int page, int pageSize, string? searchTerm, string? badges)
        {
            return _dockerImageRepository.GetDockerImages(page, pageSize, searchTerm, badges);
        }

		private async Task<DockerImage?> getImage(Guid id)
		{
			var repository = await _dockerImageRepository.GetDockerImageById(id);
			if (repository == null)
			{
				throw new NotFoundException($"Docker image with id {id.ToString()} not found.");
			}

			return repository;
		}

		public async Task DeleteDockerImage(Guid id)
		{
			DockerImage? _ = await getImage(id);

			await _dockerImageRepository.Delete(id);

		}
	}
}