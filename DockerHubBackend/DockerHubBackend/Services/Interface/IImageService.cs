using DockerHubBackend.Models;
using DockerHubBackend.Repository.Utils;

namespace DockerHubBackend.Services.Interface
{
    public interface IImageService 
    {
        Task<string> GetImageUrl(string fileName);
        Task UploadImage(string estateName, Stream fileStream);
        Task DeleteImage(string filePath);
    }
}
