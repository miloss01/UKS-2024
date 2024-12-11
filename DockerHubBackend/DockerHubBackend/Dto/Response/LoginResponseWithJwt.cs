namespace DockerHubBackend.Dto.Response
{
    public class LoginResponseWithJwt
    {
        public LoginResponse Response { get; set; }
        public string Jwt { get; set; }
    }
}
