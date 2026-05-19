namespace OWASP.Application.Services;

using Microsoft.Extensions.Logging;

using OWASP.Application.Common;
using OWASP.Application.Dtos;
using OWASP.Application.Interfaces;
using OWASP.Domain.Models;

using static OWASP.Application.Common.LoginResultCodes;

public class UserIdentityService(IHashService passwordHasher, IUserIdentityRepository repo,IUserIdentityFactory factory, ILogger<UserIdentityService> logger) : IUserIdentityService
{
    public async Task<Result<User, LoginResultCode>> GetUserByName(string userName)
    {
        var user = await repo.LoadRecordByUserNameAsync<User>(userName);
        if (user is null)
        {
            return Result<User, LoginResultCode>.Failure(LoginResultCode.UserNotFound, "User not found");
        }

        return Result<User, LoginResultCode>.Success(user, LoginResultCode.Success);
    }

    public async Task<Result<User, LoginResultCode>> GetUserByEmail(string email)
    {
        var user = await repo.LoadRecordByEmailAsync<User>(email);
        if (user is null)
        {
            return Result<User, LoginResultCode>.Failure(LoginResultCode.UserNotFound, "User not found");
        }

        return Result<User, LoginResultCode>.Success(user, LoginResultCode.Success);
    }

    public async Task<Result<User, LoginResultCode>> Login(string email, string password)
    {
        var user = await repo.LoadRecordByEmailAsync<User>(email);

        if (user is null)
        {
            logger.LogWarning("Login failed: user not found for email {Email}", email);
            return Result<User, LoginResultCode>.Failure(LoginResultCode.UserNotFound, "User not found");
        }

        var isValid = passwordHasher.VerifyHash(password, user.PasswordHash);

        if (!isValid)
        {
            logger.LogWarning("Login failed: invalid password for email {Email}", email);
            return Result<User, LoginResultCode>.Failure(LoginResultCode.InvalidCredentials, "Invalid credentials.");
        }

        user.LastActive = DateTime.Now.ToString();

        await repo.UpsertRecordsAsync(user);

        logger.LogInformation("Login successful for user {User}", user.id);

        return Result<User, LoginResultCode>.Success(user, LoginResultCode.Success);
    }

    public async Task<Result<User, LoginResultCode>> Register(RegisterRequest req)
    {
        var passwordHash = passwordHasher.GenerateHash(req.Password);

        var newUser = factory.CreateUser(req, passwordHash);

        await repo.UpsertRecordsAsync<User>(newUser);

        return Result<User, LoginResultCode>.Success(newUser, LoginResultCode.Success);
    }
}
