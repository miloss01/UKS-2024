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
    }
}
