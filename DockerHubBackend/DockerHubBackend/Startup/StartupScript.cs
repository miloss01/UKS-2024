﻿
using DockerHubBackend.Data;
using DockerHubBackend.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace DockerHubBackend.Startup
{
    public class StartupScript : IHostedService
    {
        private readonly string _configFilePath;
        private bool _generatePassword;
        private string _superAdminEmail = "";
        private string _superAdminPassword = "";

        private readonly IServiceProvider _serviceProvider;

        public StartupScript(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            var path = configuration["Startup:SuperAdminCredPath"];
            if (path == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            _configFilePath = path;
            _serviceProvider = serviceProvider;
        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            var keysToExtract = new List<string> { "generatePassword", "email"};
            LoadConfigFileData();
            if (!_generatePassword)
            {
                return Task.CompletedTask;
            }
            _superAdminPassword = PasswordGenerator.GenerateRandomPassword(16);
            SetPassword();
            CreateSuperAdminAccount();
            UpdateGeneratePassword();
            return Task.CompletedTask;
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void LoadConfigFileData()
        {
            string jsonString = File.ReadAllText(_configFilePath);
            JsonDocument doc = JsonDocument.Parse(jsonString);
            _generatePassword = doc.RootElement.GetProperty("generatePassword").GetBoolean();
            _superAdminEmail = doc.RootElement.GetProperty("email").GetString();
            if (string.IsNullOrWhiteSpace(_superAdminEmail))
            {
                throw new Exception("Invalid super admin email address");
            }
        }

        private void SetPassword()
        {
            string jsonString = File.ReadAllText(_configFilePath);
            var jsonDictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);
            if (jsonDictionary.ContainsKey("password"))
            {
                jsonDictionary["password"] = JsonDocument.Parse($"\"{_superAdminPassword}\"").RootElement;
            }
            else
            {
                jsonDictionary.Add("password", JsonDocument.Parse($"\"{_superAdminPassword}\"").RootElement);
            }
            string updatedJson = JsonSerializer.Serialize(jsonDictionary, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_configFilePath, updatedJson);
        }

        private void UpdateGeneratePassword()
        {
            string jsonString = File.ReadAllText(_configFilePath);
            var jsonDictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);
            jsonDictionary["generatePassword"] = JsonDocument.Parse("false").RootElement;
            string updatedJson = JsonSerializer.Serialize(jsonDictionary, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_configFilePath, updatedJson);
        }

        private void CreateSuperAdminAccount()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<string>>();
                var passwordHash = passwordHasher.HashPassword(String.Empty, _superAdminPassword);
                SuperAdmin superAdmin = new SuperAdmin
                {
                    Email = _superAdminEmail,
                    Password = passwordHash,
                    IsVerified = false
                };
                dbContext.Users.Add(superAdmin);
                dbContext.SaveChanges();
            }
        }
    }
}