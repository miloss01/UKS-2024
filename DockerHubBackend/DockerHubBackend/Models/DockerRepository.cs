namespace DockerHubBackend.Models
{
    public class DockerRepository : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
        public int StarCount { get; set; }
        public Badge Badge { get; set; }
        public ICollection<DockerImage> Images { get; set; } = new HashSet<DockerImage>();
        public required RepositoryOwner Owner { get; set; }
        public ICollection<Team> Teams { get; set; } = new HashSet<Team>();
    }
}
