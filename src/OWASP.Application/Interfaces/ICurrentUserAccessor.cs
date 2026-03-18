namespace OWASP.Application.Interfaces;

public interface ICurrentUserAccessor
{
    Guid GetUserId();
}
