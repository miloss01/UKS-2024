using DockerHubBackend.Dto.Request;
using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DockerHubBackend.Controllers
{
	[Route("api/repo")]
	[ApiController]
	public class RepositoryController : ControllerBase
	{
		private readonly IRepositoryService _repositoryService;

		public RepositoryController(IRepositoryService repositoryService)
		{
			_repositoryService = repositoryService;
		}

		/*[HttpGet]
		public async Task<IActionResult> GetUsersRepositories()
		{
			var repositories = await _repositoryService.getUserRepositories();
			return Ok(repositories);
		}*/

		[HttpPost]
		public IActionResult CreateRepository([FromBody] CreateRepositoryDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_repositoryService.CreateRepository(dto);
			return Ok(new { Message = "Repository created successfully!" });
		}
	}
}
