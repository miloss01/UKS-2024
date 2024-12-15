using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DockerHubBackend.Controllers
{
	[Route("api/repo")]
	[ApiController]
	public class RepositoryController
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
	}
}
