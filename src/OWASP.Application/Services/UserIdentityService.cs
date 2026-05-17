namespace OWASP.Application.Services;

using OWASP.Application.Dtos;
using OWASP.Application.Interfaces;
using OWASP.Domain.Models;

public class UserIdentityService(IHashService passwordHasher, IUserIdentityRepository repo) : IUserIdentityService
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
            return null;
        }

        var isValid = passwordHasher.VerifyHash(password, user.PasswordHash);

        if (!isValid)
        {
            return null;
        }

        //user.Token = GenerateToken();
        user.LastActive = DateTime.Now.ToString();

        await repo.UpsertRecordsAsync(user);

        return user;
    }

    public async Task<string> Register(RegisterRequest req)
    {
        var newUser = new User()
        {
            FirstName = req.FirstName,
            LastName = req.LastName,
            EmailAddress = req.EmailAddress,
            UserName = req.UserName,
            PasswordHash = passwordHasher.GenerateHash(req.Password),
        };

        await repo.UpsertRecordsAsync<User>(newUser);

        return $"{newUser.UserName} added succesfully.";
    }

    //private static string GenerateToken() => Guid.NewGuid().ToString();
}
