namespace OWASP.Identity.Settings;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Azure.Identity;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using OWASP.Application.Factories;
using OWASP.Application.Interfaces;
using OWASP.Application.Services;
using OWASP.Infrastructure.DataAccess;
using OWASP.Infrastructure.Repository;

public static class Setup
{
    public static void AddIdentityDb(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IdentityDbContext>(provider =>
        {
            var config = builder.Configuration.GetSection("CosmosDb");
            var cosmosDb = new IdentityDbContext(
                config["EndpointUrl"]!,
                config["PrimaryKey"]!,
                config["DatabaseName"]!,
                config["IdentityContainerName"]!);

            return cosmosDb;
        });
    }

    public static void AddKeyVaultSecrets(this WebApplicationBuilder builder)
    {
        var keyVaultUri = builder.Configuration["KeyVault:VaultUri"];
        builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri!), new DefaultAzureCredential());
    }

    public static IServiceCollection AddAuthenticationAndBearer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()
            ?? throw new InvalidOperationException("JwtSettings configuration is missing.");

        if (string.IsNullOrWhiteSpace(jwtSettings.Secret))
        {
            throw new InvalidOperationException("JwtSettings:Secret is missing.");
        }

        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("Security");

                        logger.LogWarning(
                            "SecurityEvent: JwtAuthenticationFailed Path={Path} Error={Error}",
                            context.HttpContext.Request.Path,
                            context.Exception.Message);

                        return Task.CompletedTask;
                    },

                    OnChallenge = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("Security");

                        logger.LogWarning(
                            "SecurityEvent: JwtChallenge Path={Path} Error={Error} Description={Description}",
                            context.HttpContext.Request.Path,
                            context.Error,
                            context.ErrorDescription);

                        return Task.CompletedTask;
                    },

                    OnTokenValidated = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("Security");

                        var name = context.Principal?.Identity?.Name;
                        var email = context.Principal?.FindFirst(ClaimTypes.Email)?.Value;
                        var userId = context.Principal?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                        logger.LogInformation(
                            "SecurityEvent: JwtTokenValidated Path={Path} Name={Name} Email={Email} UserId={UserId}",
                            context.HttpContext.Request.Path,
                            name,
                            email,
                            userId);

                        return Task.CompletedTask;
                    },
                };
            });

        return services;
    }

    public static IServiceCollection AddUserIdentityServices(this IServiceCollection services)
    {
        services.AddTransient<IUserIdentityFactory, UserIdentityFactory>();
        services.AddScoped<IUserIdentityService, UserIdentityService>();
        services.AddScoped<IHashService, HashingService>();
        services.AddScoped<IUserIdentityRepository, UserIdentityRepository>();

        return services;
    }
}
