using Amazon.S3.Model;
using DockerHubBackend.Data;
using DockerHubBackend.Dto.Request;
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

        public async Task<Organization?> AddOrganization(AddOrganizationDto dto)
        {
            var user = await _userRepository.GetUserByEmail(dto.OwnerEmail);
            if (user == null) {
                return null;
            }

            StandardUser standardUser = new StandardUser { Email = user.Email, IsVerified = user.IsVerified, Password = user.Password };

            var organization = new Organization
            {
                Name = dto.Name,
                Description = dto.Description,
                ImageLocation = "",
                OwnerId = user.Id,
                Owner = standardUser,
                Members = new HashSet<StandardUser>
                {
                    standardUser,
                }
            };

            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();
            return organization;        
        }
    }
}
