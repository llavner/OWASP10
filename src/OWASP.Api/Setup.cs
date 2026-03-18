namespace OWASP.Api;

using OWASP.Api.Auth;
using OWASP.Application.Interfaces;

public static class Setup
{
    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserAccessor, StubCurrentUserAccessor>();
    }
}
