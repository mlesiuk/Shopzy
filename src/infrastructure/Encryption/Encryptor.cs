using Shopzy.Application.Abstractions.Interfaces;
using System.Security.Cryptography;

namespace Shopzy.Infrastructure.Encryption;

public sealed class Encryptor : IEncryptor
{
    private readonly int _saltSize = 256 / 8;
    private readonly int _keySize = 512 / 8;
    private readonly int _iterations = 1000;
    private readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA512;

    public string Encrypt(string input)
    {
        var salt = RandomNumberGenerator.GetBytes(_saltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, _iterations, _hashAlgorithmName, _keySize);
        return $"{Convert.ToBase64String(hash)}{Convert.ToBase64String(salt)}";
    }

    public bool Verify(string input, string? hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return false;
        }

        var saltBytesTemplate = Convert.ToBase64String(RandomNumberGenerator.GetBytes(_saltSize));
        var saltBytes = string.Concat(hash.Skip(hash.Length - saltBytesTemplate.Length));
        var salt = Convert.FromBase64String(saltBytes);
        var hashWithoutSalt = string.Concat(hash.Take(hash.Length - saltBytesTemplate.Length));
        var encryptedInput = Convert.ToBase64String(Rfc2898DeriveBytes.Pbkdf2(input, salt, _iterations, _hashAlgorithmName, _keySize));
        return hashWithoutSalt.Equals(encryptedInput);
    }
}
