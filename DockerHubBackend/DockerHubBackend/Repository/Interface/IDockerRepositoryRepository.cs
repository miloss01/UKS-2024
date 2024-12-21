using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Utils;

namespace DockerHubBackend.Repository.Interface
{
    public interface IDockerRepositoryRepository : ICrudRepository<DockerRepository>
    {
        public DockerRepository GetFullDockerRepositoryById(Guid id);
        public List<DockerRepository> GetStarRepositoriesForUser(Guid userId);
        public List<DockerRepository> GetPrivateRepositoriesForUser(Guid userId);
        public List<DockerRepository> GetOrganizationRepositoriesForUser(Guid userId);
        public List<DockerRepository> GetAllRepositoriesForUser(Guid userId);
        public void AddStarRepository(Guid userId, Guid repositoryId);
    }
}
