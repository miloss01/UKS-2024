namespace DockerHubBackend.Models
{
    public class DockerImage : BaseEntity
    {
        public required DockerRepository Repository { get; set; }
        public DateTime? LastPush;
        public List<string> Tags { get; set; } = new List<string>();
    }
}
