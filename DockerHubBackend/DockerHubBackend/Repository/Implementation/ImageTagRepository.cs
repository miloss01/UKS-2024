using DockerHubBackend.Data;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Repository.Utils;
using Microsoft.EntityFrameworkCore;

namespace DockerHubBackend.Repository.Implementation
{
	public class ImageTagRepository : CrudRepository<ImageTag>, IImageTagRepository
	{
		public ImageTagRepository(DataContext context) : base(context)
		{
		}

		public async Task<ImageTag?> GetByDockerImageIdAndName(Guid imageId, string tagName)
		{
			return await _context.ImageTags
						.FirstOrDefaultAsync(tag => tag.DockerImageId == imageId && tag.Name == tagName);
		}
	}
}
