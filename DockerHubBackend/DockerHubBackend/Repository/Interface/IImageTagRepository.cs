using DockerHubBackend.Models;
using DockerHubBackend.Repository.Utils;

namespace DockerHubBackend.Repository.Interface
{
	public interface IImageTagRepository : ICrudRepository<ImageTag>
	{
		Task<ImageTag?> GetByDockerImageIdAndName(Guid imageId, string tagName);
		Task<ICollection<ImageTag>> GetByDockerImageId(Guid imageId);
	}
}
