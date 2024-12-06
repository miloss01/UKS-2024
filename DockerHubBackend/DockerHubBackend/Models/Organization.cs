﻿using System.ComponentModel.DataAnnotations.Schema;

namespace DockerHubBackend.Models
{
    public class Organization : RepositoryOwner
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string ImageLocation { get; set; }
        public ICollection<StandardUser> Members { get; set; } = new HashSet<StandardUser>();
        public required StandardUser Owner { get; set; }
        public ICollection<DockerRepository> Repositories { get; set; } = new HashSet<DockerRepository>();
        public ICollection<Team> Teams { get; set; } = new HashSet<Team>();
    }
}
