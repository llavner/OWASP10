namespace OWASP.Application.Services;

using Microsoft.Extensions.Logging;

using OWASP.Application.Dtos;
using OWASP.Application.Interfaces;
using OWASP.Domain.Models;

public class UserIdentityService(IHashService passwordHasher, IUserIdentityRepository repo,IUserIdentityFactory factory, ILogger<UserIdentityService> logger) : IUserIdentityService
{
    public async Task<User?> GetUserByName(string userName)
    {
        var user = await repo.LoadRecordByUserNameAsync<User>(userName);
        if (user is null)
        {
            return null;
        }

        return user;
    }

    public async Task<User?> GetUserByToken(string token)
    {
        var user = await repo.LoadRecordByTokenAsync<User>(token);
        if (user is null)
        {
            return null;
        }

        return user;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await repo.LoadRecordByEmailAsync<User>(email);
        if (user is null)
        {
            return null;
        }

        return user;
    }

    public async Task<User?> Login(string email, string password)
    {
        var user = await repo.LoadRecordByEmailAsync<User>(email);

        if (user is null)
        {
            logger.LogWarning("Login failed: user not found for email {Email}", email);
            return null;
        }

        var isValid = passwordHasher.VerifyHash(password, user.PasswordHash);

        if (!isValid)
        {
            logger.LogWarning("Login failed: invalid password for email {Email}", email);
            return null;
        }

        user.LastActive = DateTime.Now.ToString();

        await repo.UpsertRecordsAsync(user);

        logger.LogInformation("Login successful for user {User}", user.id);

        return user;
    }

    public async Task Register(RegisterRequest req)
    {
        var passwordHash = passwordHasher.GenerateHash(req.Password);

        var newUser = factory.CreateUser(req, passwordHash);

        await repo.UpsertRecordsAsync<User>(newUser);
    }
}
