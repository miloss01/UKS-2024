namespace DockerHubBackend.Models
{
    public class TeamPermission : BaseEntity
    {
        public PermissionType permission {  get; set; }
        public required Team Team { get; set; }
        public required DockerRepository Repository { get; set; }
    }
}
