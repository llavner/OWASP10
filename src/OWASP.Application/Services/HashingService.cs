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
        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

    public bool VerifyHash(string password, string passwordHash)
    {
        if (!TryParseHash(passwordHash, out var expectedHash, out var salt))
        {
            return false;
        }

        var actualHash = HashPassword(password, salt);
        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }

    private static bool TryParseHash(string passwordHash, out byte[] hash, out byte[] salt)
    {
        hash = [];
        salt = [];

        var parts = passwordHash.Split('-');
        if (parts.Length != 2)
        {
            return false;
        }

        try
        {
            hash = Convert.FromHexString(parts[0]);
            salt = Convert.FromHexString(parts[1]);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static byte[] HashPassword(string password, byte[] salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        using var argon2 = new Argon2id(passwordBytes)
        {
            MemorySize = MemorySize,
            DegreeOfParallelism = Parallelism,
            Iterations = Iterations,
            Salt = salt,
        };

        return argon2.GetBytes(HashSize);
    }
}
