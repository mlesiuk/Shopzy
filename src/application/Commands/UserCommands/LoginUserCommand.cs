using FluentValidation;
using MediatR;
using OneOf;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Application.Dtos;
using Shopzy.Application.Exceptions;
using Shopzy.Domain.Entities;

namespace Shopzy.Application.Commands.UserCommands;

public sealed class LoginUserCommand : IRequest<OneOf<LoginDto, ValidationException, UserNotFoundException>>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, OneOf<LoginDto, ValidationException, UserNotFoundException>>
{
    private readonly IEncryptor _encryptor;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IUserRepository _repository;
    private readonly IValidator<LoginUserCommand> _validator;

    public LoginUserCommandHandler(
        IEncryptor encryptor,
        IUserRepository repository,
        IJwtGenerator jwtGenerator,
        IValidator<LoginUserCommand> validator)
    {
        _encryptor = encryptor;
        _jwtGenerator = jwtGenerator;
        _repository = repository;
        _validator = validator;
    }

    public async Task<OneOf<LoginDto, ValidationException, UserNotFoundException>> Handle(LoginUserCommand request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        var failures = validationResult.Errors?.ToList();
        if (failures is not null && failures!.Any())
        {
            return new ValidationException(failures);
        }

        var userName = request.Email;
        User? existingEntity = null;
        if (!string.IsNullOrWhiteSpace(userName))
        {
            existingEntity = await _repository.FindByEmailAsync(userName);
        }
        if (existingEntity is null && !string.IsNullOrWhiteSpace(request.Username))
        {
            userName = request.Username;
            existingEntity = await _repository.FindByUsernameAsync(userName);
        }

        if (existingEntity is null)
        {
            return new UserNotFoundException(userName);
        }

        if (_encryptor.Verify(request.Password, existingEntity.PasswordHash) == false)
        {
            return new ValidationException("Invalid password");
        }

        var usersRoles = await _repository.GetRolesAsync(existingEntity.Id);
        var jwt = _jwtGenerator.Generate(existingEntity, usersRoles);
        return new LoginDto(jwt.AccessToken, jwt.ExpiresIn);
    }
}
