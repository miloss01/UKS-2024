using System.ComponentModel.DataAnnotations.Schema;

namespace DockerHubBackend.Models
{
    public abstract class BaseUser : BaseEntity
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public DateTime? LastPasswordChangeDate { get; set; }
        public required bool IsVerified { get; set; }
    }
}
