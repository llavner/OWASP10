using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;

using OWASP.Application.Handlers;

namespace OWASP3.Api;

public static class Setup
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = DemoAuthHandler.SchemeName;
                options.DefaultChallengeScheme = DemoAuthHandler.SchemeName;
            })
            .AddScheme<AuthenticationSchemeOptions, DemoAuthHandler>(
                DemoAuthHandler.SchemeName, _ => { });

        services
            .AddAuthorizationBuilder()
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build())
            .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

        return services;
    }

    public static IServiceCollection AddSwaggerGenServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // 1) Demo UserId header -> ger en input i Swagger "Authorize"
            c.AddSecurityDefinition("DemoUserId", new OpenApiSecurityScheme
            {
                Description = "Demo header auth. Skriv t.ex. user-a eller user-b.",
                Name = "X-Demo-UserId",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "DemoUserId",
            });

            // 2) Demo Role header (valfri) -> Admin för admin-endpoints
            c.AddSecurityDefinition("DemoRole", new OpenApiSecurityScheme
            {
                Description = "Valfri roll. Skriv t.ex. Admin för att passera AdminOnly policy.",
                Name = "X-Demo-Role",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "DemoRole",
            });

            // 3) Kräv båda headers i Swagger (de blir valbara i UI).
            // Swagger kommer då skicka med dem i requests när du klickat Authorize.
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "DemoUserId",
                },
            },
            Array.Empty<string>()
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "DemoRole",
                },
            },
            Array.Empty<string>()
        },
    });
        });

        return services;
    }
}
