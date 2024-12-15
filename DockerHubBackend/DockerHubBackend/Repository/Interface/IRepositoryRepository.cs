using DockerHubBackend.Models;
using DockerHubBackend.Repository.Utils;

namespace DockerHubBackend.Repository.Interface
{
	public interface IRepositoryRepository : ICrudRepository<DockerRepository>
	{
	}
}
