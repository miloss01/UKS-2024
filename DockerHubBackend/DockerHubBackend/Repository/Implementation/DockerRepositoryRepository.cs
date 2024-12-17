using DockerHubBackend.Data;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Repository.Utils;
using Microsoft.EntityFrameworkCore;

namespace DockerHubBackend.Repository.Implementation
{
    public class DockerRepositoryRepository : CrudRepository<DockerRepository>, IDockerRepositoryRepository
    {
        public DockerRepositoryRepository(DataContext context) : base(context) { }
    }
}
