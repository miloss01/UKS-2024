using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;

namespace DockerHubBackend.Services.Interface
{
    public interface IDockerRepositoryService
    {
        public DockerRepository GetDockerRepositoryById(Guid id);

        public Task<DockerRepositoryDTO> CreateDockerRepository(CreateRepositoryDto createRepositoryDto);

		public Task<DockerRepositoryDTO> ChangeDockerRepositoryDescription(Guid id, string description);
		public Task<DockerRepositoryDTO> ChangeDockerRepositoryVisibility(Guid id, bool visibility);

        public Task DeleteDockerRepository(Guid id);
	}
}
