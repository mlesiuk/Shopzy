using FluentValidation;
using MassTransit;
using MediatR;
using OneOf;
using Shopzy.Contracts.Events;
using Shopzy.Application.Exceptions;
using Shopzy.Application.Utils;
using Shopzy.Domain.Entities;
using Shopzy.Domain.ValueObjects;
using Shopzy.Application.Dtos;
using Shopzy.Application.Abstractions.Interfaces;

namespace Shopzy.Application.Commands.UserCommands;

public sealed class CreateUserCommand 
    : IRequest<OneOf<Guid, ValidationException, AlreadyExistsException>>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IReadOnlyList<AddressDto> Addressess { get; set; } = new List<AddressDto>();
}

public sealed class CreateUserCommandHandler 
    : IRequestHandler<CreateUserCommand, OneOf<Guid, ValidationException, AlreadyExistsException>>
{
    private readonly IEncryptor _encryptor;
    private readonly IPublishEndpoint _publisher;
    private readonly IUserRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateUserCommand> _validator;

    public CreateUserCommandHandler(
        IEncryptor encryptor,
        IPublishEndpoint publisher,
        IUserRepository repository,
        IUnitOfWork unitOfWork,
        IValidator<CreateUserCommand> validator)
    {
        _encryptor = encryptor;
        _publisher = publisher;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<OneOf<Guid, ValidationException, AlreadyExistsException>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        var failures = validationResult.Errors?.ToList();
        if (failures is not null && failures!.Any())
        {
            return new ValidationException(failures);
        }

        var existingEntity = await _repository
            .FindByUsernameOrEmailAsync(request.Username, request.Email);
        if (existingEntity is not null)
        {
            return new ValidationException(new AlreadyExistsException().Message);
        }

        var passwordHash = _encryptor.Encrypt(request.Password);

        var user = User.Create(
            request.Username,
            request.Email,
            passwordHash);

        user.AddAddressess(request.Addressess.ToAddressList());
        _repository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new UserCreatedEvent
        {
            Id = Guid.NewGuid(),
            UserName = user.Username,
            Email = user.Email
        }, cancellationToken);

        return user.Id;
    }
}
