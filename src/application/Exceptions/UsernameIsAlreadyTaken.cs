namespace Shopzy.Application.Exceptions;

public class UsernameIsAlreadyTaken : Exception
{
    public UsernameIsAlreadyTaken()
        : base("Username is already taken")
    {
    }

    public UsernameIsAlreadyTaken(string username)
        : base($"Username '{username}' is already taken")
    {
    }
}
