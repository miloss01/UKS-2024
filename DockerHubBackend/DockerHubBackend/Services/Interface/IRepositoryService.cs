
using DockerHubBackend.Dto.Request;

namespace DockerHubBackend.Services.Interface
{
	public interface IRepositoryService
	{
		Task getUserRepositories();
		Task CreateRepository(CreateRepositoryDto dto);
	}
}
