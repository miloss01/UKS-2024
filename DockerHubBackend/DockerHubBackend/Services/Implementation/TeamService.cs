using System.ComponentModel;
using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Exceptions;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;

namespace DockerHubBackend.Services.Implementation
{
    public class TeamService : ITeamService
    {   
        private readonly ITeamRepository _repository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDockerRepositoryRepository _dockerRepositoryRepository;
        
        public TeamService(ITeamRepository repository, IOrganizationRepository organizationRepository,
            IUserRepository userRepository, IDockerRepositoryRepository dockerRepositoryRepository) 
        {
            _repository = repository;
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
            _dockerRepositoryRepository = dockerRepositoryRepository;
        }

        public async Task<ICollection<TeamDto>?> GetTeams(Guid organizationId)
        {
            return await _repository.GetByOrganizationId(organizationId);
        }

        public async Task<TeamDto?> Get(Guid id)
        {
            Team? team = await _repository.Get(id);
            if (team == null) { throw new NotFoundException("Team not found."); }
            return new TeamDto(team);
        }

        public async Task<TeamDto> Create(TeamDto teamDto)
        {   
            Organization? organization = await _organizationRepository.Get(teamDto.OrganizationId);
            if (organization == null) { throw new NotFoundException("Organization does not exist."); }
            Team? t = await _repository.GetByOrgIdAndTeamName(organization.Id, teamDto.Name);
            if (t != null) throw new BadRequestException("Team with chosen name already exists."); 
            Team team = teamDto.ToTeam(organization);
            team.Members = await toStandardUsers(teamDto.Members);
            await _repository.Create(team);
            
            Team returnedTeam = await _repository.GetByName(teamDto.Name);
            ICollection<EmailDto> memberDtos = new HashSet<EmailDto>();
            foreach (StandardUser user in team.Members) { memberDtos.Add(user.ToMemberDto()); }
           
            return new TeamDto(returnedTeam.Id, returnedTeam.Name, returnedTeam.Description, memberDtos);
            
        }

        public async Task<TeamDto> AddMembers(Guid teamId, ICollection<EmailDto> memberDtos)
        {
            Team? team = await _repository.Get(teamId);
            if (team == null) { throw new NotFoundException("Team does not exist."); }
            team.Members = await toStandardUsers(memberDtos);  // TODO: add check for members (this should be done after organization merge)
            Team? updatedTeam = await _repository.Update(team);
            if (updatedTeam == null) { throw new BadRequestException("Error occured while adding members. Addition aborted."); }
            return new TeamDto(updatedTeam);
        }

        public async Task<TeamPermissionResponseDto> AddPermissions(TeamPermissionRequestDto teamPermissionDto)
        {
            TeamPermission? teamPerm = _repository.GetTeamPermission(teamPermissionDto.RepositoryId, teamPermissionDto.TeamId);
            if (teamPerm != null) { throw new BadRequestException("Team-Permission already exists."); }
            DockerRepository? dr = await _dockerRepositoryRepository.Get(teamPermissionDto.RepositoryId);
            if (dr == null) { throw new NotFoundException("Repositoy not found."); }
            Team? t = await _repository.Get(teamPermissionDto.TeamId);
            if (t == null) { throw new NotFoundException("Team not found."); }
            TeamPermission tp = new TeamPermission
            {
                TeamId = teamPermissionDto.TeamId,
                RepositoryId = teamPermissionDto.RepositoryId,
                Team = t,
                Repository = dr,
                permission = toPermissionType(teamPermissionDto.Permission),
            };
            _repository.AddPermissions(tp);

            return new TeamPermissionResponseDto(tp);
        }

        public async Task<TeamDto> Update(TeamDto teamDto, Guid id)
        {
            Team? team = await _repository.Get(id);
            if (team == null) { throw new NotFoundException("Team not found."); }
            team.Name = teamDto.Name;
            team.Description = teamDto.Description;
            team = await _repository.Update(team);
            if (team == null) { throw new NotFoundException("Team not found. Update aborted."); }
            return new TeamDto(team);
        }

        public async Task<TeamDto> Delete(Guid id)
        {
            Team? team = await _repository.Delete(id);
            if (team == null) { throw new NotFoundException("Team not found."); }
            return new TeamDto(team);
        }

        public async Task<ICollection<TeamPermission>> GetTeamPermissions(Guid id)
        {
            return await _repository.GetTeamPermissions(id);
        }

        private async Task<ICollection<StandardUser>> toStandardUsers(ICollection<EmailDto> memberDtos)
        {
            ICollection<StandardUser?> members = new HashSet<StandardUser?>();
            foreach (EmailDto memberDto in memberDtos)
            {
                BaseUser? baseUser = await _userRepository.GetUserByEmail(memberDto.Email);
                StandardUser user = (StandardUser)baseUser;
                members.Add(user);
            }
            return members;
        }

        private PermissionType toPermissionType(string input)
        {
            if (Enum.TryParse<PermissionType>(input, true, out PermissionType permissionType))
            {
                return permissionType;
            }
            else
            {
                throw new NotFoundException("Chosen permission type not found.");
            }
        }

    }
}
