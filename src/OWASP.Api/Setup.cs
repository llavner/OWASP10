namespace OWASP.Api;

using OWASP.Api.Auth;
using OWASP.Application.Interfaces;
using OWASP.Infrastructure.Repository;

public static class Setup
{
    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserAccessor, StubCurrentUserAccessor>();
        services.AddSingleton<IOvertimeEntryRepository, InMemoryRepository>();
    }
}
