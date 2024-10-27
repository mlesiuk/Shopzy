using Microsoft.AspNetCore.Identity;
using Shopzy.Application.Dtos;
using Shopzy.Domain.Entities;

namespace Shopzy.Application.Exceptions;

public sealed class AlreadyExistsException : Exception
{
    public AlreadyExistsException(AuditableEntity entity)
        : base($"Specified {nameof(entity)} already exists")
    {
    }

    public AlreadyExistsException(Dto dto)
        : base($"Specified {nameof(dto)} already exists")
    {
    }

    public AlreadyExistsException(IdentityUser<Guid> entity)
        : base($"Specified {nameof(entity)} already exists")
    {

    }

    public AlreadyExistsException(string message)
        : base(message)
    {
    }

    public AlreadyExistsException()
        : base($"User with given data already exists")
    {
    }

    public AlreadyExistsException(UsernameIsAlreadyTaken usernameIsAlreadyTaken)
        : base(usernameIsAlreadyTaken.Message)
    {
    }

    public AlreadyExistsException(EmailAddressIsAlreadyTaken emailAddressIsAlreadyTaken)
        : base(emailAddressIsAlreadyTaken.Message)
    {
    }
}
