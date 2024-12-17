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

            var dockerRepositoryDto = new DockerRepositoryDTO(dockerRepository);

            return Ok(dockerRepositoryDto);
        }

		[HttpPost]
		public IActionResult CreateRepository([FromBody] CreateRepositoryDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_dockerRepositoryService.CreateDockerRepository(dto);
			return Ok(new { Message = "Repository created successfully!" });
		}
	}
}
