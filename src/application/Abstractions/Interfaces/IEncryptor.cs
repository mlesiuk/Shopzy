namespace Shopzy.Application.Abstractions.Interfaces;

public interface IEncryptor
{
    string Encrypt(string input);
    bool Verify(string input, string? hash);
}
