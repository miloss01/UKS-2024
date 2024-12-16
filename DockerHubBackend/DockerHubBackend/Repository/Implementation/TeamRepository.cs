using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using DockerHubBackend.Data;
using DockerHubBackend.Dto.Response;
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

        public async Task<ICollection<TeamDto>> GetTeamsByOrganizationId(Guid organizationId)
        {
            var teams = await _context.Teams
                .Where(team => team.OrganizationId == organizationId)
                .Include(team => team.Members)
                .Select(team => new TeamDto
                {
                    Name = team.Name,
                    Description = team.Description,
                    Members = team.Members.Select(member => new MemberDto
                    {
                        Email = member.Email,
                    }).ToList()
                })
                .ToListAsync();
            return new Collection<TeamDto>(teams);

        }
    }
}
