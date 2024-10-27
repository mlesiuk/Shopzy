using FluentValidation;
using MediatR;
using OneOf;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Application.Dtos;
using Shopzy.Application.Exceptions;
using Shopzy.Domain.Entities;

namespace Shopzy.Application.Commands.RoleCommands;

public sealed class CreateRoleCommand 
    : Role, IRequest<OneOf<RoleDto, ValidationException, AlreadyExistsException>>
{
}

public sealed class CreateRoleCommandHandler 
    : IRequestHandler<CreateRoleCommand, OneOf<RoleDto, ValidationException, AlreadyExistsException>>
{
    private readonly IRoleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<Role> _validator;

    public CreateRoleCommandHandler(
        IRoleRepository repository,
        IUnitOfWork unitOfWork,
        IValidator<Role> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<OneOf<RoleDto, ValidationException, AlreadyExistsException>> Handle(
        CreateRoleCommand request, 
        CancellationToken cancellationToken = default)
    {
        var entity = request;
        var validationResult = await _validator.ValidateAsync(entity, cancellationToken);
        var failures = validationResult.Errors?.ToList();
        if (failures is not null && failures!.Any())
        {
            return new ValidationException(failures);
        }

        var catgoryAlreadyExists = await _repository.FindByNameAsync(entity.Name);
        if (catgoryAlreadyExists != null)
        {
            return new AlreadyExistsException(entity);
        }

        _repository.Add(request);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new RoleDto(request.Name);
    }
}
