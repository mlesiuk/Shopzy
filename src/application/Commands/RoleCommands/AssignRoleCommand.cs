using FluentValidation;
using MediatR;
using OneOf;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Application.Dtos;
using Shopzy.Application.Exceptions;
using Shopzy.Domain.Entities;

namespace Shopzy.Application.Commands.RoleCommands;

public sealed class AssignRoleCommand : IRequest<OneOf<UserRoleDto, ValidationException, NotFoundException>>
{
    public User? User { get; init; }
    public Role? Role { get; init; }
}

public sealed class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, OneOf<UserRoleDto, ValidationException, NotFoundException>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AssignRoleCommand> _validator;

    public AssignRoleCommandHandler(
        IUserRepository userRepository,
        IRoleRepository rolesRepository,
        IUnitOfWork unitOfWork,
        IValidator<AssignRoleCommand> validator)
    {
        _userRepository = userRepository;
        _roleRepository = rolesRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<OneOf<UserRoleDto, ValidationException, NotFoundException>> Handle(AssignRoleCommand request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        var failures = validationResult.Errors?.ToList();
        if (failures is not null && failures!.Any())
        {
            return new ValidationException(failures);
        }

        var user = await _userRepository.FindByUsernameAsync(request.User!.Username);
        if (user is null)
        {
            return new NotFoundException(nameof(request.User));
        }

        var role = await _roleRepository.FindByNameAsync(request.Role!.Name);
        if (role is null)
        {
            return new NotFoundException(nameof(request.Role));
        }

        user.AddRole(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new UserRoleDto();
    }
}
