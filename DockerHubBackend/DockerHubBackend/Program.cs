using DockerHubBackend.Config;
using DockerHubBackend.Data;
using DockerHubBackend.Filters;
using DockerHubBackend.Repository.Implementation;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Security;
using DockerHubBackend.Services.Implementation;
using DockerHubBackend.Services.Interface;
using DockerHubBackend.Startup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AWS"));

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtCookieName = builder.Configuration["JWT:CookieName"];

// Add services to the container.

// Authentication
builder.Services.AddScoped<IPasswordHasher<string>, PasswordHasher<string>>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var claimsPrincipal = context.Principal;
            Guid userId;
            if(!Guid.TryParse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), out userId))
            {
                context.Fail("Token is invalid");
            }
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<DataContext>();
            var user = await dbContext.Users.FindAsync(userId);

            if (user == null || user.LastPasswordChangeDate > context.SecurityToken.ValidFrom)
            {
                context.Fail("Token is invalid due to password change");
            }
        }
    };
});
builder.Services.AddSingleton<IJwtHelper,JwtHelper>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CORS_CONFIG",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                      });
});

// Database
builder.Services.AddDbContext<DataContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVerificationTokenRepository, VerificationTokenRepository>();
builder.Services.AddScoped<IDockerImageRepository, DockerImageRepository>();
builder.Services.AddScoped<IDockerRepositoryRepository, DockerRepositoryRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();


// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IRandomTokenGenerator, RandomTokenGenerator>();
builder.Services.AddScoped<IDockerImageService, DockerImageService>();
builder.Services.AddScoped<IDockerRepositoryService, DockerRepositoryService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionHandler>();
});

builder.Services.AddHostedService<StartupScript>();

var app = builder.Build();

await DatabaseContextSeed.SeedDataAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CORS_CONFIG");

app.MapControllers();

app.Run();
