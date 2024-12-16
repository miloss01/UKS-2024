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
            Organization? organization = await _organizationRepository.Get(teamDto.OrganizationId);
            Team team = teamDto.ToTeam(organization);
            team.Members = await toStandardUsers(teamDto.Members);
            await _repository.Create(team);
            
            Team returnedTeam = await _repository.GetByName(teamDto.Name);
            ICollection<MemberDto> memberDtos = new HashSet<MemberDto>();
            foreach (StandardUser user in team.Members) { memberDtos.Add(user.ToMemberDto()); }
           
            return new TeamResponseDto(returnedTeam.Name, returnedTeam.Description, memberDtos);
            
        }

        public async Task<TeamResponseDto> AddMembers(Guid teamId, ICollection<MemberDto> memberDtos)
        {
            Team? team = await _repository.Get(teamId);
            team.Members = await toStandardUsers(memberDtos);
            Team? updatedTeam = await _repository.Update(team);

            return new TeamResponseDto(updatedTeam);
        }

        public Task<Team> ChangePersmissions(Guid teamId, PermissionType permissionType)
        {
            throw new NotImplementedException();
        }

        public async Task<Team> Update(Team team)
        {
            throw new NotImplementedException();
        }

        private async Task<ICollection<StandardUser>> toStandardUsers(ICollection<MemberDto> memberDtos)
        {
            ICollection<StandardUser?> members = new HashSet<StandardUser?>();
            foreach (MemberDto memberDto in memberDtos)
            {
                BaseUser? baseUser = await _userRepository.GetUserByEmail(memberDto.Email);
                StandardUser user = (StandardUser)baseUser;
                members.Add(user);
            }
            return members;
        }

    }
}
