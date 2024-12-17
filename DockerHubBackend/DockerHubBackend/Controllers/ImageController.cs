using DockerHubBackend.Dto.Request;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Security;
using DockerHubBackend.Services.Implementation;
using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace DockerHubBackend.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPut("get-image-url")]
        public async Task<IActionResult> GetImageUrl([FromBody] ImageDto dto)
        {
            if (string.IsNullOrEmpty(dto.FileName))
            {
                return BadRequest("File name is required.");
            }

            var url = await _imageService.GetImageUrlAsync(dto.FileName);
            if (url == null)
            {
                return NotFound("Image not found.");
            }

            return Ok(new { ImageUrl = url });
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] string filePath, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using (var stream = file.OpenReadStream())
            {
                await _imageService.UploadImageAsync(filePath, stream);
            }

            return Ok("Image uploaded successfully.");
        }
    }
}
