using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Utils;

namespace DockerHubBackend.Repository.Interface
{
    public interface IDockerRepositoryRepository : ICrudRepository<DockerRepository>
    {
        Task<DockerRepository?> GetDockerRepositoryById(Guid id);

        Task<List<DockerRepository>?> GetRepositoriesByUserOwnerId(Guid id);

		public DockerRepository GetFullDockerRepositoryById(Guid id);
    }
}
