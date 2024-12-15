using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
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
    [Route("api/dockerImages")]
    [ApiController]
    public class DockerImageController : ControllerBase
    {
        private readonly IDockerImageService _dockerImageService;

        public DockerImageController(IDockerImageService dockerImageService)
        {
            _dockerImageService = dockerImageService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetDockerImages(int page, int pageSize, string? searchTerm, string? badges)
        {
            var pagedDockerImages = _dockerImageService.GetDockerImages(page, pageSize, searchTerm, badges);
            var pageDTO = new PageDTO<DockerImageDTO>(
                                pagedDockerImages.Data.Select(img => new DockerImageDTO
                                {
                                    RepositoryName = img.Repository.Name,
                                    RepositoryId = img.Repository.Id.ToString(),
                                    Badge = img.Repository.Badge.ToString(),
                                    Description = img.Repository.Description,
                                    CreatedAt = img.CreatedAt.ToString(),
                                    LastPush = img.LastPush == null ? "nema lastpush" : img.LastPush.ToString(),
                                    ImageId = img.Id.ToString(),
                                    StarCount = img.Repository.StarCount,
                                    Tags = img.Tags,
                                    Owner = img.Repository.OrganizationOwner == null ? img.Repository.UserOwner.Email : img.Repository.OrganizationOwner.Name
                                }).ToList(),
                                pagedDockerImages.TotalNumberOfElements
                            );

            return Ok(pageDTO);
        }
    }
}
