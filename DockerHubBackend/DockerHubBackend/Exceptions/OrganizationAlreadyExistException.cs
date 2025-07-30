namespace DockerHubBackend.Exceptions
{
    public class OrganizationAlreadyExistsException : Exception
    {
        public OrganizationAlreadyExistsException(string name)
            : base($"Name '{name}' is already taken") { }
    }
}
