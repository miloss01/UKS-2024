using DockerHubBackend.Data;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Repository.Utils;
using Microsoft.EntityFrameworkCore;

namespace DockerHubBackend.Repository.Implementation
{
    public class DockerRepositoryRepository : CrudRepository<DockerRepository>, IDockerRepositoryRepository
    {
        public DockerRepositoryRepository(DataContext context) : base(context) { }

        public async Task<DockerRepository> GetRepo(Guid id)
        {
            return await _context.DockerRepositories
                .Where(repo => repo.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
