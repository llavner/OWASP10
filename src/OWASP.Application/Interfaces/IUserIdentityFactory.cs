using OWASP.Application.Dtos;
using OWASP.Domain.Models;

namespace OWASP.Application.Interfaces;

public interface IUserIdentityFactory
{
    User CreateUser(RegisterRequest req, string passwordHash);
}
