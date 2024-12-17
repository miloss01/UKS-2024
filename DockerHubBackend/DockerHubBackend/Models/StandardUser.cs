
namespace DockerHubBackend.Models
{
    public class StandardUser : BaseUser
    {
        public Badge Badge { get; set; }
        public ICollection<DockerRepository> StarredRepositories { get; set; } = new HashSet<DockerRepository>();
        public ICollection<DockerRepository> MyRepositories { get; set; } = new HashSet<DockerRepository>();
        public ICollection<Organization> MyOrganizations { get; set; } = new HashSet<Organization>();
        public ICollection<Organization> MemberOrganizations { get; set; } = new HashSet<Organization>();
        public ICollection<Team> Teams { get; set; } = new HashSet<Team>();

        
    }
}
