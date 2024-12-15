using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Utils;

namespace DockerHubBackend.Repository.Interface
{
    public interface IDockerImageRepository : ICrudRepository<DockerRepository>
    {
        public PageDTO<DockerImage> GetDockerImages(int page, int pageSize, string? searchTerm, string? badges);
    }
}