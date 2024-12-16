using DockerHubBackend.Data;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Repository.Utils;

namespace DockerHubBackend.Repository.Implementation
{
    public class OrganizationRepository : CrudRepository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(DataContext context) : base(context) { }
    }
}
