using DockerHubBackend.Data;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Repository.Utils;
using Microsoft.EntityFrameworkCore;

namespace DockerHubBackend.Repository.Implementation
{
    public class OrganizationRepository : CrudRepository<BaseUser>, IOrganizationRepository
    {
        public OrganizationRepository(DataContext context) : base(context) { }

        public async Task<BaseUser?> AddOrganization(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }
    }
}
