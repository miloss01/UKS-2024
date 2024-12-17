using DockerHubBackend.Models;
using DockerHubBackend.Repository.Utils;

namespace DockerHubBackend.Services.Interface
{
    public interface IImageService 
    {
        Task<string> GetImageUrlAsync(string fileName);
        Task UploadImageAsync(string estateName, Stream fileStream);
    }
}
