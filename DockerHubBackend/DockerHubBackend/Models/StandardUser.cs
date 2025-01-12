using System.Text.Json.Serialization;
using DockerHubBackend.Dto.Response;

namespace DockerHubBackend.Models
{
    public class StandardUser : BaseUser
    {
        public ICollection<DockerRepository> StarredRepositories { get; set; } = new HashSet<DockerRepository>();
        public ICollection<DockerRepository> MyRepositories { get; set; } = new HashSet<DockerRepository>();
        public ICollection<Organization> MyOrganizations { get; set; } = new HashSet<Organization>();
        public ICollection<Organization> MemberOrganizations { get; set; } = new HashSet<Organization>();
        public ICollection<Team> Teams { get; set; } = new HashSet<Team>();

        public StandardUser() { }

        public StandardUser(string email, string username, string password, string location)
        {
            Email = email;
            Username = username;
            Password = password;
            Location = location;
            Badge = Badge.NoBadge;
            CreatedAt = DateTime.UtcNow;
        }

        public EmailDto ToMemberDto()
        {
            return new EmailDto { Email = Email };
        }

    }
}
