using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Services.Interface;

namespace DockerHubBackend.Services.Implementation
{
    public class TeamService : ITeamService
    {   
        private readonly ITeamRepository _repository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;
        public TeamService(ITeamRepository repository, IOrganizationRepository organizationRepository, IUserRepository userRepository) 
        {
            _repository = repository;
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
        }

        public async Task<ICollection<TeamResponseDto>?> GetTeams(Guid organizationId)
        {
            return await _repository.GetByOrganizationId(organizationId);
        }

        public async Task<TeamResponseDto> Create(TeamRequestDto teamDto)
        {
            Organization organization = await _organizationRepository.Get(teamDto.OrganizationId);
            Team team = teamDto.ToTeam(organization);

            ICollection<StandardUser> members = new HashSet<StandardUser>();
            foreach (MemberDto memberDto in teamDto.Members)
            {
                BaseUser? baseUser =  await _userRepository.GetUserByEmail(memberDto.Email);
                StandardUser user = (StandardUser)baseUser;
                members.Add(user);
            }
            team.Members = members;

            await _repository.Create(team);
            
            Team returnedTeam = await _repository.GetByName(teamDto.Name);
            ICollection<MemberDto> memberDtos = new HashSet<MemberDto>();
            foreach (StandardUser user in team.Members) { memberDtos.Add(user.ToMemberDto()); }
           
            return new TeamResponseDto
            {
                Name = returnedTeam.Name,
                Description = returnedTeam.Description,
                Members = memberDtos,
            };
            
        }

        public Team AddMembers(Guid teamId, ICollection<StandardUser> members)
        {
            throw new NotImplementedException();
        }

        public Team ChangePersmissions(Guid teamId, PermissionType permissionType)
        {
            throw new NotImplementedException();
        }

        public Team Update(Team team)
        {
            throw new NotImplementedException();
        }
    }
}
