using DockerHubBackend.Data;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Repository.Utils;

namespace DockerHubBackend.Repository.Implementation
{
	public class RepositoryRepository : CrudRepository<DockerRepository>, IRepositoryRepository
	{
		public RepositoryRepository(DataContext context) : base(context) { }

	}
}
