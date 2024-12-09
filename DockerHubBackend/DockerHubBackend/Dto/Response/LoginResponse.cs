using System.Text.Json;

namespace DockerHubBackend.Dto.Response
{
    public class LoginResponse
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserRole { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
