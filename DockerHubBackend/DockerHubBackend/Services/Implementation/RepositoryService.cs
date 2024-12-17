using DockerHubBackend.Dto.Request;
using DockerHubBackend.Models;
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

		

		public Task CreateRepository(CreateRepositoryDto dto)
		{
			var newRepository = new DockerRepository
			{
				Name = dto.Name,
				Description = dto.Description,
				IsPublic = dto.Visibility == "public",
			};

			_repositoryRepository.Create(newRepository);

			// Log or save to DB here in real implementation
			Console.WriteLine($"Repository {newRepository.Name} created successfully!");
			return Task.CompletedTask;
		}
	}
}

