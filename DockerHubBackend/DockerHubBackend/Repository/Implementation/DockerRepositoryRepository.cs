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

        public DockerRepository GetFullDockerRepositoryById(Guid id)
        {
            return _context.DockerRepositories
                .AsQueryable()
                .Include(dockerRepository => dockerRepository.UserOwner)
                .Include(dockerRepository => dockerRepository.OrganizationOwner)
                .Include(dockerRepository => dockerRepository.Images)
                .FirstOrDefault(dockerRepository => dockerRepository.Id == id);
        }

        public List<DockerRepository> GetStarRepositoriesForUser(Guid userId)
        {
            return _context.Users
                .OfType<StandardUser>()
                .Where(user => !user.IsDeleted)
                .AsQueryable()
                .Include(user => user.StarredRepositories)
                    .ThenInclude(starRepository => starRepository.UserOwner)
                .Include(user => user.StarredRepositories)
                    .ThenInclude(starRepository => starRepository.OrganizationOwner)
                .Include(user => user.StarredRepositories)
                    .ThenInclude(starRepository => starRepository.Images)
                .First(user => user.Id == userId)
                .StarredRepositories
                .ToList();
        }

        public List<DockerRepository> GetPrivateRepositoriesForUser(Guid userId)
        {
            return _context.DockerRepositories
                .AsQueryable()
                .Include(dockerRepository => dockerRepository.UserOwner)
                .Include(dockerRepository => dockerRepository.OrganizationOwner)
                    .ThenInclude(organization => organization.Owner)
                .Include(dockerRepository => dockerRepository.Images)
                .Where(dockerRepository => !dockerRepository.IsDeleted)
                .Where(dockerRepository => dockerRepository.UserOwnerId == userId ||
                                           dockerRepository.OrganizationOwner.OwnerId == userId)
                .Where(dockerRepository => !dockerRepository.IsPublic)
                .ToList();
        }
    }
}
