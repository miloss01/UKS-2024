using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Services.Interface;

namespace DockerHubBackend.Services.Implementation
{
	public class RepositoryService : IRepositoryService
	{
		private readonly IRepositoryRepository _repositoryRepository;

		public RepositoryService(IRepositoryRepository repositoryRepository)
		{
			_repositoryRepository = repositoryRepository;
		}

		public Task getUserRepositories()
		{
			throw new NotImplementedException();
		}
	}
}
