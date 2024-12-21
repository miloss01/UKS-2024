using DockerHubBackend.Data;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Exceptions;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Repository.Utils;
using Microsoft.EntityFrameworkCore;

namespace DockerHubBackend.Repository.Implementation
{
    public class DockerRepositoryRepository : CrudRepository<DockerRepository>, IDockerRepositoryRepository
    {
        public DockerRepositoryRepository(DataContext context) : base(context) { }

		public async Task<DockerRepository?> GetDockerRepositoryById(Guid id)
		{
			return await _context.DockerRepositories.FirstOrDefaultAsync(repo => repo.Id == id);
		}

        public async Task<List<DockerRepository>?> GetRepositoriesByUserOwnerId(Guid id)
        { 
            return await _context.DockerRepositories
				                 .Where(repo => repo.UserOwnerId == id)
						         .ToListAsync();
		}

		public DockerRepository GetFullDockerRepositoryById(Guid id)
        {
            return _context.DockerRepositories
                .AsQueryable()
                .Include(dockerRepository => dockerRepository.UserOwner)
                .Include(dockerRepository => dockerRepository.OrganizationOwner)
                .Include(dockerRepository => dockerRepository.Images)
                .FirstOrDefault(dockerRepository => dockerRepository.Id == id);
        }
    }
}
