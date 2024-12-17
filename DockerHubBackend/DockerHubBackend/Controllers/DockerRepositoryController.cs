using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Exceptions;
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
    }
}
