using System.Security.Cryptography;

namespace Fitness_Tracker.Services;

///<summary> Salted a PBKDF2 password hashing, so passwords are never stored in clear text.</summary>

public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;

    public static string GenerateSalt() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(SaltSize));

    public static string Hash(string password, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password, saltBytes, Iterations, HashAlgorithmName.SHA256, HashSize);
        return Convert.ToBase64String(hash);
    }

    public static bool Verify(string password, string salt, string expectedHash)
    {
        byte[] actual = Convert.FromBase64String(Hash(password, salt));
        byte[] expected = Convert.FromBase64String(expectedHash);
        return CryptographicOperations.FixedTimeEquals(actual, expected);

    }

}

