namespace OWASP.Application.Interfaces;

using OWASP.Application.Dtos;
using OWASP.Domain.Models;

public interface IUserIdentityService
{
    Task<User?> GetUserByName(string userName);

    Task<User?> GetUserByToken(string token);

    Task<User?> GetUserByEmail(string email);

    Task<string?> Login(string username, string password);

    Task<string> Register(RegisterRequest regReq);
}
