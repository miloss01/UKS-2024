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

            var dockerRepositoryDto = new DockerRepositoryDTO(dockerRepository);

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
            var starRepositoriesDto = starRepositories.Select(starRepository => new DockerRepositoryDTO(starRepository));

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
            var privateRepositoriesDto = privateRepositories.Select(privateRepository => new DockerRepositoryDTO(privateRepository));

            return Ok(privateRepositoriesDto);
        }

        [HttpPatch("star/{userId}/{repositoryId}")]
        [AllowAnonymous]
        public IActionResult AddStarRepository(string userId, string repositoryId)
        {
            var parsed = Guid.TryParse(userId, out var userGuid);

            if (!parsed)
            {
                throw new NotFoundException("User not found. Bad user id.");
            }

            parsed = Guid.TryParse(repositoryId, out var repositoryGuid);

            if (!parsed)
            {
                throw new NotFoundException("Repository not found. Bad repository id.");
            }

            _dockerRepositoryService.AddStarRepository(userGuid, repositoryGuid);

            return NoContent();
        }

        [HttpGet("star/notallowed/{id}")]
        [AllowAnonymous]
        public IActionResult GetNotAllowedRepositoriesToStarForUser(string id)
        {
            var parsed = Guid.TryParse(id, out var userId);

            if (!parsed)
            {
                throw new NotFoundException("User not found. Bad user id.");
            }

            var starRepositories = _dockerRepositoryService.GetStarRepositoriesForUser(userId);
            var myRepositories = _dockerRepositoryService.GetAllRepositoriesForUser(userId);
            var organizationRepositories = _dockerRepositoryService.GetOrganizationRepositoriesForUser(userId);
            
            var allGuids = starRepositories
                .Concat(myRepositories)
                .Concat(organizationRepositories)
                .Select(repository => repository.Id.ToString());

            return Ok(allGuids);
        }
    }
}
