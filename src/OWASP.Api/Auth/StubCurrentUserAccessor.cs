using OWASP.Application.Interfaces;

namespace OWASP.Api.Auth;

public class StubCurrentUserAccessor : ICurrentUserAccessor
{
    public Guid GetUserId() => throw new NotImplementedException();
}
