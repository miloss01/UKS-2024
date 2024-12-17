using System.ComponentModel.DataAnnotations;

namespace DockerHubBackend.Dto.Request
{
	public class CreateRepositoryDto
	{
		[Required(ErrorMessage = "Name is required.")]
		[StringLength(50, ErrorMessage = "Name must be less than 50 characters.")]
		public required string Name { get; set; }

		public required string Description { get; set; }

		[Required(ErrorMessage = "Namespace is required.")]
		public required string NamespaceR { get; set; }

		[Required(ErrorMessage = "Visibility is required.")]
		[RegularExpression("public|private", ErrorMessage = "Visibility must be 'public' or 'private'.")]
		public required string Visibility { get; set; }

		public CreateRepositoryDto()
		{
		}

		public CreateRepositoryDto(string name, string description, string namespaceR, string visibility)
		{
			Name = name;
			Description = description;
			NamespaceR = namespaceR;
			Visibility = visibility;
		}
	}
}
