namespace OWASP.Application.Services;

using System.Security.Cryptography;
using System.Text;

using Konscious.Security.Cryptography;

using OWASP.Application.Interfaces;

public class HashingService : IHashService
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int MemorySize = 65536;
    private const int Parallelism = 6;
    private const int Iterations = 4;

    public string GenerateHash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = HashPassword(password, salt);
        return hash;
    }

    public bool VerifyHash(string password, string passwordHash)
    {
        var parts = passwordHash.Split('-');
        var hash = Convert.FromHexString(parts[0]);
        var salt = Convert.FromHexString(parts[1]);
        var outputHash = HashPassword(password, salt);
        return outputHash.Equals(passwordHash);
    }

    private string HashPassword(string password, byte[] salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        using var argon2 = new Argon2id(passwordBytes)
        {
            MemorySize = MemorySize,
            DegreeOfParallelism = Parallelism,
            Iterations = Iterations,
            Salt = salt,
        };
        var hash = argon2.GetBytes(HashSize);
        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }
}
