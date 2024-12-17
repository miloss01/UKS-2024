using DockerHubBackend.Models;
using DockerHubBackend.Repository.Utils;

namespace DockerHubBackend.Repository.Interface
{
    public interface IDockerRepositoryRepository : ICrudRepository<DockerRepository>
    {
        Task<DockerRepository> GetRepo(Guid id);
    }
}
