namespace OWASP.Application.Factories;

using OWASP.Application.Dtos;
using OWASP.Application.Interfaces;
using OWASP.Domain.Models;

public class UserIdentityFactory : IUserIdentityFactory
{
    public User CreateUser(RegisterRequest request, string passwordHash)
    {
        return new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            EmailAddress = request.EmailAddress,
            UserName = request.UserName,
            PasswordHash = passwordHash,
        };
    }
}
