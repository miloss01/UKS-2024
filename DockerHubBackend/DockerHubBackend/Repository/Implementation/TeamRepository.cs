using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using DockerHubBackend.Data;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Repository.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace DockerHubBackend.Repository.Implementation
{
    public class TeamRepository : CrudRepository<Team>, ITeamRepository
    {

        public TeamRepository(DataContext context) : base(context) { }

        public async Task<ICollection<Team>> GetTeamsByOrganizationId(Guid organizationId)
        {
            var teams = await _context.Teams.Where(team => team.OrganizationId == organizationId).Include(team => team.Members).ToListAsync();
            return new Collection<Team>(teams);

        }
    }
}
