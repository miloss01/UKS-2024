using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Exceptions;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Security;
using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace DockerHubBackend.Controllers
{
    [Route("api/dockerRepositories")]
    [ApiController]
    public class DockerRepositoryController : ControllerBase
    {
        private readonly IDockerRepositoryService _dockerRepositoryService;

        public DockerRepositoryController(IDockerRepositoryService dockerRepositoryService)
        {
            _dockerRepositoryService = dockerRepositoryService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult GetDockerRepositoryById(string id)
        {
            var parsed = Guid.TryParse(id, out var repositoryId);

            if (!parsed)
            {
                throw new NotFoundException("Repository not found. Bad repository id.");
            }

            var dockerRepository = _dockerRepositoryService.GetDockerRepositoryById(repositoryId);
            Console.WriteLine(dockerRepository.Images == null);

            var dockerRepositoryDto = new DockerRepositoryDTO
            {
                Id = dockerRepository.Id.ToString(),
                Name = dockerRepository.Name,
                Description = dockerRepository.Description,
                Badge = dockerRepository.Badge.ToString(),
                CreatedAt = dockerRepository.CreatedAt.ToString(),
                IsPublic = dockerRepository.IsPublic,
                StarCount = dockerRepository.StarCount,
                Owner = dockerRepository.OrganizationOwner == null ? dockerRepository.UserOwner.Email : dockerRepository.OrganizationOwner.Name,
                Images = dockerRepository.Images.Select(img => new DockerImageDTO
                {
                    RepositoryName = img.Repository.Name,
                    RepositoryId = img.Repository.Id.ToString(),
                    Badge = img.Repository.Badge.ToString(),
                    Description = img.Repository.Description,
                    CreatedAt = img.CreatedAt.ToString(),
                    LastPush = img.LastPush != null ? img.LastPush.ToString() : null,
                    ImageId = img.Id.ToString(),
                    StarCount = img.Repository.StarCount,
                    Tags = img.Tags,
                    Owner = img.Repository.OrganizationOwner == null ? img.Repository.UserOwner.Email : img.Repository.OrganizationOwner.Name
                }).ToList()
            };

            return Ok(dockerRepositoryDto);
        }

        [HttpGet("star/{id}")]
        [AllowAnonymous]
        public IActionResult GetStarRepositoriesForUser(string id)
        {
            var parsed = Guid.TryParse(id, out var userId);

            if (!parsed)
            {
                throw new NotFoundException("User not found. Bad user id.");
            }

            var starRepositories = _dockerRepositoryService.GetStarRepositoriesForUser(userId);
            var starRepositoriesDto = starRepositories.Select(starRepository => new DockerRepositoryDTO
            {
                Id = starRepository.Id.ToString(),
                Name = starRepository.Name,
                Description = starRepository.Description,
                Badge = starRepository.Badge.ToString(),
                CreatedAt = starRepository.CreatedAt.ToString(),
                IsPublic = starRepository.IsPublic,
                StarCount = starRepository.StarCount,
                Owner = starRepository.OrganizationOwner == null ? starRepository.UserOwner.Email : starRepository.OrganizationOwner.Name,
                Images = starRepository.Images.Select(img => new DockerImageDTO
                {
                    RepositoryName = img.Repository.Name,
                    RepositoryId = img.Repository.Id.ToString(),
                    Badge = img.Repository.Badge.ToString(),
                    Description = img.Repository.Description,
                    CreatedAt = img.CreatedAt.ToString(),
                    LastPush = img.LastPush != null ? img.LastPush.ToString() : null,
                    ImageId = img.Id.ToString(),
                    StarCount = img.Repository.StarCount,
                    Tags = img.Tags,
                    Owner = img.Repository.OrganizationOwner == null ? img.Repository.UserOwner.Email : img.Repository.OrganizationOwner.Name
                }).ToList()
            });

            return Ok(starRepositoriesDto);
        }

        [HttpGet("private/{id}")]
        [AllowAnonymous]
        public IActionResult GetPrivateRepositoriesForUser(string id)
        {
            var parsed = Guid.TryParse(id, out var userId);

            if (!parsed)
            {
                throw new NotFoundException("User not found. Bad user id.");
            }

            var privateRepositories = _dockerRepositoryService.GetPrivateRepositoriesForUser(userId);
            var privateRepositoriesDto = privateRepositories.Select(privateRepository => new DockerRepositoryDTO
            {
                Id = privateRepository.Id.ToString(),
                Name = privateRepository.Name,
                Description = privateRepository.Description,
                Badge = privateRepository.Badge.ToString(),
                CreatedAt = privateRepository.CreatedAt.ToString(),
                IsPublic = privateRepository.IsPublic,
                StarCount = privateRepository.StarCount,
                Owner = privateRepository.OrganizationOwner == null ? privateRepository.UserOwner.Email : privateRepository.OrganizationOwner.Name,
                Images = privateRepository.Images.Select(img => new DockerImageDTO
                {
                    RepositoryName = img.Repository.Name,
                    RepositoryId = img.Repository.Id.ToString(),
                    Badge = img.Repository.Badge.ToString(),
                    Description = img.Repository.Description,
                    CreatedAt = img.CreatedAt.ToString(),
                    LastPush = img.LastPush != null ? img.LastPush.ToString() : null,
                    ImageId = img.Id.ToString(),
                    StarCount = img.Repository.StarCount,
                    Tags = img.Tags,
                    Owner = img.Repository.OrganizationOwner == null ? img.Repository.UserOwner.Email : img.Repository.OrganizationOwner.Name
                }).ToList()
            });

            return Ok(privateRepositoriesDto);
        }
    }
}
