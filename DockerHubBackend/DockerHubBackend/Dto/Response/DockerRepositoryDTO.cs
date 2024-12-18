using DockerHubBackend.Models;
using System.Text.Json;

namespace DockerHubBackend.Dto.Response
{
    public class DockerRepositoryDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public int StarCount { get; set; }
        public string Badge { get; set; }
        public ICollection<DockerImageDTO> Images { get; set; }
        public string Owner { get; set; }
        public string CreatedAt { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}