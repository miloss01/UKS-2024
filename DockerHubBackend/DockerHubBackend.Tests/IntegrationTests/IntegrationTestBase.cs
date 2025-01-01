﻿using DockerHubBackend.Data;
using DockerHubBackend.Tests.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

public class IntegrationTestBase : IAsyncLifetime
{
    protected readonly HttpClient _httpClient;
    protected readonly IServiceScopeFactory _scopeFactory;

    public IntegrationTestBase()
    {
        // Kreiramo WebApplicationFactory za testiranje API-ja
        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                    services.AddDbContext<DataContext>(options =>
                        options.UseNpgsql(configuration.GetConnectionString("TestConnection")));
                });
            });

        _httpClient = factory.CreateClient();
        _scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
    }

    // Resetovanje baze pre svakog testa
    public async Task InitializeAsync()
    {
        await ResetDatabase();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    // Metoda za resetovanje baze
    private async Task ResetDatabase()
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        await dbContext.Database.EnsureDeletedAsync(); // Brisanje baze
        await dbContext.Database.EnsureCreatedAsync(); // Ponovno kreiranje baze
    }

    // Metoda za ubacivanje podataka u bazu
    protected async Task SeedDatabaseAsync(Func<DataContext, Task> seeder)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        await seeder(dbContext);
        await dbContext.SaveChangesAsync();
    }

    // Metoda za direktan pristup bazi iz testa
    protected DataContext GetDbContext()
    {
        var scope = _scopeFactory.CreateScope();
        return scope.ServiceProvider.GetRequiredService<DataContext>();
    }
}