using DockerHubBackend.Data;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Repository.Utils;
using DockerHubBackend.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace DockerHubBackend.Repository.Implementation
{
    public class OrganizationRepository : CrudRepository<BaseUser>, IOrganizationRepository
    {
        public OrganizationRepository(DataContext context) : base(context) { }

        public async Task<Organization> AddOrganization(Organization organization)
        {
            // Dodavanje organizacije u DbSet
            _context.Organizations.Add(organization);

            // Cuvanje promena u bazi podataka
            await _context.SaveChangesAsync();

            return organization; // Vraca novododatu organizaciju (moyete vracati i samo ID, ako je potrebno)          
        }
    }
}
