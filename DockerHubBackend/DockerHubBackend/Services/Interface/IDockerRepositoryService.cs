using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;

namespace DockerHubBackend.Services.Interface
{
    public interface IDockerRepositoryService
    {
        public DockerRepository GetDockerRepositoryById(Guid id);
        public List<DockerRepository> GetStarRepositoriesForUser(Guid userId);
        public List<DockerRepository> GetPrivateRepositoriesForUser(Guid userId);
        public List<DockerRepository> GetOrganizationRepositoriesForUser(Guid userId);
        public List<DockerRepository> GetAllRepositoriesForUser(Guid userId);
        public void AddStarRepository(Guid userId, Guid repositoryId);
        public void RemoveStarRepository(Guid userId, Guid repositoryId);
    }
}
