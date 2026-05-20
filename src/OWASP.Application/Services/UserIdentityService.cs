namespace OWASP.Application.Services;

using Microsoft.Extensions.Logging;

using OWASP.Application.Common;
using OWASP.Application.Dtos;
using OWASP.Application.Interfaces;
using OWASP.Domain.Models;

using static OWASP.Application.Common.LoginResultCodes;

public class UserIdentityService(
    IHashService passwordHasher,
    IUserIdentityRepository repo,
    IUserIdentityFactory factory,
    ILogger<UserIdentityService> logger) : IUserIdentityService
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
            logger.LogWarning("SecurityEvent: LoginFailed_UserNotFound Email={Email}", email);
            return Result<User, LoginResultCode>.Failure(LoginResultCode.UserNotFound, "User not found");
        }

        var isValid = passwordHasher.VerifyHash(password, user.PasswordHash);

        if (!isValid)
        {
            logger.LogWarning("SecurityEvent: LoginFailed_InvalidPassword Email={Email}", email);
            return Result<User, LoginResultCode>.Failure(LoginResultCode.InvalidCredentials, "Invalid credentials.");
        }

        user.LastActive = DateTime.Now.ToString();
        await repo.UpsertRecordsAsync(user);

        logger.LogInformation("SecurityEvent: LoginSucceeded UserId={UserId}", user.id);

        return Result<User, LoginResultCode>.Success(user, LoginResultCode.Success);
    }

    public async Task<Result<User, LoginResultCode>> Register(RegisterRequest req)
    {
        logger.LogInformation("SecurityEvent: RegistrationAttempt Email={Email} UserName={UserName}", req.EmailAddress, req.UserName);

        try
        {
            var passwordHash = passwordHasher.GenerateHash(req.Password);
            var newUser = factory.CreateUser(req, passwordHash);

            await repo.UpsertRecordsAsync<User>(newUser);

            logger.LogInformation("SecurityEvent: RegistrationSucceeded UserId={UserId}", newUser.id);

            return Result<User, LoginResultCode>.Success(newUser, LoginResultCode.Success);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "SecurityEvent: RegistrationFailed Email={Email}", req.EmailAddress);
            return Result<User, LoginResultCode>.Failure(LoginResultCode.UnknownError, "Registration failed.");
        }
    }
}
