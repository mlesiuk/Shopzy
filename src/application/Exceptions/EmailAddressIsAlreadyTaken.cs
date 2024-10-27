namespace Shopzy.Application.Exceptions;

public class EmailAddressIsAlreadyTaken : Exception
{
    public EmailAddressIsAlreadyTaken()
        : base("E-mail address is already taken")
    {
    }

    public EmailAddressIsAlreadyTaken(string email)
        : base($"E-mail address '{email}' is already taken")
    {
    }
}
