using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Exceptions;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Security;
using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace DockerHubBackend.Services.Implementation
{
    public class DockerRepositoryService : IDockerRepositoryService
    {
        private readonly IDockerRepositoryRepository _dockerRepositoryRepository;
		private readonly IUserRepository _userRepository;
		private readonly IOrganizationRepository _organizationRepository;

        public DockerRepositoryService(IDockerRepositoryRepository dockerRepositoryRepository, IUserRepository userRepository, IOrganizationRepository organizationRepository)
        {
            _dockerRepositoryRepository = dockerRepositoryRepository;
			_userRepository = userRepository;
			_organizationRepository = organizationRepository;
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

			Console.WriteLine(repository.UserOwner);

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

			if (repository.IsPublic == visibility)
				throw new BadRequestException("The new visibility value is the same as the current value.");

			repository.IsPublic = visibility;

			// Save changes to the repository
			await _dockerRepositoryRepository.Update(repository);

			// Map the updated repository to a DTO (assuming a mapping method or library)
			var updatedRepositoryDto = new DockerRepositoryDTO(repository);

			// Return the updated DTO
			return updatedRepositoryDto;
		}

		public async Task<StandardUser?> getUser(string id)
		{
			var parsed = Guid.TryParse(id, out var userId);

			if (!parsed)
			{
				return null;
			}
			try
			{
				var user = await _userRepository.GetUserById(userId);
				return (StandardUser?)user;
			}
			catch (Exception)
			{
				return null;
			}
		}


		public async Task<Organization?> getOrganization(string id)
		{
			var parsed = Guid.TryParse(id, out var orgId);

			if (!parsed)
			{
				return null;
			}
			return await _organizationRepository.GetOrganizationById(orgId);
		}

		public async Task<DockerRepositoryDTO> CreateDockerRepository(CreateRepositoryDto createRepositoryDto)
		{
			Console.WriteLine(createRepositoryDto);
			// Get either User or Organization
			var userOwner = await getUser(createRepositoryDto.Owner);
			var organizationOwner = await getOrganization(createRepositoryDto.Owner);
			if ((userOwner == null) && (organizationOwner == null))
			{
				throw new ArgumentException("Invalid namespace name. It can be either an organization name or username.");
			}
			// Create the repo
			var newRepository = new DockerRepository
			{
				Name = createRepositoryDto.Name,
				Description = createRepositoryDto.Description,
				IsPublic = createRepositoryDto.IsPublic,
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

		public async Task<List<DockerRepositoryDTO>> GetRepositoriesByUserId(Guid id)
		{
			var repositories = await _dockerRepositoryRepository.GetRepositoriesByUserOwnerId(id);

			if (repositories == null)
				return new List<DockerRepositoryDTO> { };

			return repositories.Select(repo => new DockerRepositoryDTO(repo)).ToList();
		}



	}
}