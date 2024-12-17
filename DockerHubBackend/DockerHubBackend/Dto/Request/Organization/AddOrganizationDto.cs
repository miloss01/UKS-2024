using System.Text.Json;

namespace DockerHubBackend.Dto.Request
{
    public class AddOrganizationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public AddOrganizationDto() { }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
