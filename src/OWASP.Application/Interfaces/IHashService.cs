namespace OWASP.Application.Interfaces;

public interface IHashService
{
    string GenerateHash(string password);

    bool VerifyHash(string password, string passwordHash);
}
