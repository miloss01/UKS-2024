using System.ComponentModel.DataAnnotations.Schema;

namespace DockerHubBackend.Models
{
    public class DockerImage : BaseEntity
    {
        public required Guid DockerRepositoryId { get; set; }

        [ForeignKey(nameof(DockerRepositoryId))]
        public required DockerRepository Repository { get; set; }
        public DateTime? LastPush { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
