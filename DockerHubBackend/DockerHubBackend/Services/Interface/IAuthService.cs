﻿using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;

namespace DockerHubBackend.Services.Interface
{
    public interface IAuthService
    {
        Task<LoginResponseWithJwt> Login(LoginCredentialsDto credentials);
    }
}