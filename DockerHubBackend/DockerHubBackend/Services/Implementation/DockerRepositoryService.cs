using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Exceptions;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Security;
using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace DockerHubBackend.Services.Implementation
{
    public class DockerRepositoryService : IDockerRepositoryService
    {
        private readonly IDockerRepositoryRepository _dockerRepositoryRepository;
		private readonly IUserRepository _userRepository;

        public DockerRepositoryService(IDockerRepositoryRepository dockerRepositoryRepository)
        {
            _dockerRepositoryRepository = dockerRepositoryRepository;
        }

		private async Task<DockerRepository?> getRepository(Guid id)
		{
			var repository = await _dockerRepositoryRepository.GetDockerRepositoryById(id);
			if (repository == null)
			{
				throw new NotFoundException($"Docker repository with id {id.ToString()} not found.");
			}

			return repository;
		}

		public async Task<DockerRepositoryDTO> ChangeDockerRepositoryDescription(Guid id, string description)
		{
			DockerRepository? repository = await getRepository(id);

			repository.Description = description;

			// Save changes to the repository
			await _dockerRepositoryRepository.Update(repository);

			// Map the updated repository to a DTO (assuming a mapping method or library)
			var updatedRepositoryDto = new DockerRepositoryDTO(repository);

			// Return the updated DTO
			return updatedRepositoryDto;
		}


		public async Task<DockerRepositoryDTO> ChangeDockerRepositoryVisibility(Guid id, bool visibility)
		{
			DockerRepository? repository = await getRepository(id);

			repository.IsPublic = visibility;

			// Save changes to the repository
			await _dockerRepositoryRepository.Update(repository);

			// Map the updated repository to a DTO (assuming a mapping method or library)
			var updatedRepositoryDto = new DockerRepositoryDTO(repository);

			// Return the updated DTO
			return updatedRepositoryDto;
		}

		public async Task<StandardUser?> getUser(string repoNamespace)
		{
			var user = await _userRepository.GetUserByEmail(repoNamespace);
			try
			{
				return (StandardUser?)user;

			}
			catch (Exception)
			{
				return null;
			}

		}

		public Organization? getOrganization(string repoNamespace)
		{
			return null;
		}

		public async Task<DockerRepositoryDTO> CreateDockerRepository(CreateRepositoryDto createRepositoryDto)
		{
			// Get either User or Organization
			var userOwner = await getUser(createRepositoryDto.NamespaceR);
			var organizationOwner = getOrganization(createRepositoryDto.NamespaceR);
			if ((userOwner == null) ^ (organizationOwner == null))
			{
				throw new ArgumentException("Invalid namespace name. It can be either an organization name or username.");
			}

			// Create the repo
			var newRepository = new DockerRepository
			{
				Name = createRepositoryDto.Name,
				Description = createRepositoryDto.Description,
				IsPublic = createRepositoryDto.Visibility.ToLower() == "public",
				StarCount = 0,
				Badge = Badge.VefifiedPublisher,
				Images = new HashSet<DockerImage>(),
				Teams = new HashSet<Team>(),
				UserOwner = userOwner,
				OrganizationOwner = organizationOwner
			};
			await _dockerRepositoryRepository.Create(newRepository);

			// Map the saved entity to a DTO
			var repositoryDto = new DockerRepositoryDTO(newRepository);

			// Return the created DTO
			return repositoryDto;
		}

		public async Task DeleteDockerRepository(Guid id)
		{
			DockerRepository? _ = await getRepository(id);

			await _dockerRepositoryRepository.Delete(id);

		}

		public DockerRepository GetDockerRepositoryById(Guid id)
        {
            var dockerRepository = _dockerRepositoryRepository.GetFullDockerRepositoryById(id);

            if (dockerRepository == null)
            {
                throw new NotFoundException($"Docker repository with id {id.ToString()} not found.");
            }

            return dockerRepository;
        }
    }
}