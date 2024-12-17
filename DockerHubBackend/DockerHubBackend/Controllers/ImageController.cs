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

        // Endpoint za generisanje URL-a za sliku
        [HttpGet("get-image-url")]
        public async Task<IActionResult> GetImageUrl([FromQuery] string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name is required.");
            }

            var url = await _imageService.GetImageUrlAsync(fileName);
            if (url == null)
            {
                return NotFound("Image not found.");
            }

            return Ok(new { ImageUrl = url });
        }

        // Endpoint za upload slike na S3
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromQuery] string estateName, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Preuzimanje fajla kao stream
            using (var stream = file.OpenReadStream())
            {
                await _imageService.UploadImageAsync(estateName, stream);
            }

            return Ok("Image uploaded successfully.");
        }
    }
}
