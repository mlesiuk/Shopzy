namespace Shopzy.Application.Exceptions;

public sealed class UserNotFoundException : Exception
{
    public UserNotFoundException(string user) : base($"User {user} was not found")
    {
    }
}
