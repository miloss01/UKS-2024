using Amazon.S3.Model;
using DockerHubBackend.Data;
using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Repository.Utils;
using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DockerHubBackend.Repository.Implementation
{
    public class OrganizationRepository : CrudRepository<BaseUser>, IOrganizationRepository
    {
        private IUserRepository _userRepository;
        public OrganizationRepository(DataContext context, IUserRepository userRepository) : base(context) {
            this._userRepository = userRepository;
        }

        public async Task<Guid?> AddOrganization(AddOrganizationDto dto)
        {
            var user = await _userRepository.GetUserByEmail(dto.OwnerEmail);
            if (user == null) {
                return null;
            }

            var standardUser = user as StandardUser;
            if (standardUser == null)
            {
                return null;
            }

            var organization = new Organization
            {
                Name = dto.Name,
                Description = dto.Description,
                ImageLocation = dto.ImageLocation,
                OwnerId = user.Id,
                Owner = standardUser,
                Members = new HashSet<StandardUser>
                {
                    standardUser,
                }
            };

            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();
            return organization.Id;        
        }

        public async Task<List<OrganizationOwnershipDto>?> GetUserOrganizations(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return null;
            }

            var organizations = await _context.Organizations
                .Where(o => !o.IsDeleted &&
                    (o.OwnerId == user.Id ||
                     o.Members.Any(m => m.Id == user.Id)))
                .Select(o => new OrganizationOwnershipDto
                {
                    Id = o.Id,
                    Name = o.Name,
                    Description = o.Description,
                    ImageLocation = o.ImageLocation,
                    CreatedAt = o.CreatedAt,
                    IsOwner = o.OwnerId == user.Id 
                })
                .ToListAsync();

            return organizations;
            }

    }
}
