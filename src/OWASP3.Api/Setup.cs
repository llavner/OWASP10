namespace OWASP3.Api;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

using OWASP.Application.Handlers;

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
}
