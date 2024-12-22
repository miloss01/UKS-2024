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

        public List<DockerRepository> GetOrganizationRepositoriesForUser(Guid userId)
        {
            return _context.Organizations
                .Include(organization => organization.Members)
                .Include(organization => organization.Repositories)
                .Include(organization => organization.Owner)
                .Where(organization => organization.Members.Any(member => member.Id == userId) ||
                                       organization.Owner.Id == userId)
                .SelectMany(organization => organization.Repositories)
                .ToList();
        }

        public List<DockerRepository> GetAllRepositoriesForUser(Guid userId)
        {
            return _context.Users
                .OfType<StandardUser>()
                .Include(user => user.MyRepositories)
                .First(user => user.Id == userId)
                .MyRepositories
                .ToList();
        }

        public void AddStarRepository(Guid userId, Guid repositoryId)
        {
            var user = _context.Users.OfType<StandardUser>().FirstOrDefault(user => user.Id == userId);
            var repository = _context.DockerRepositories.Find(repositoryId);
            user.StarredRepositories.Add(repository);
            repository.StarCount += 1;
            _context.SaveChanges();
        }

        public void RemoveStarRepository(Guid userId, Guid repositoryId)
        {
            var user = _context.Users
                .OfType<StandardUser>()
                .Include(user => user.StarredRepositories)
                .FirstOrDefault(user => user.Id == userId);
            var repository = _context.DockerRepositories.Find(repositoryId);
            user.StarredRepositories.Remove(repository);
            repository.StarCount -= 1;
            _context.SaveChanges();
        }
    }
}
