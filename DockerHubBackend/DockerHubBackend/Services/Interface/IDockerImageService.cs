using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;
using static System.Net.Mime.MediaTypeNames;

namespace DockerHubBackend.Services.Interface
{
    public interface IDockerImageService
    {
        public PageDTO<DockerImage> GetDockerImages(int page, int pageSize, string? searchTerm, string? badges);
		public Task DeleteDockerImage(Guid id);
        public Task DeleteTagForDockerImage(Guid imageId, string tagName);

	}
}
