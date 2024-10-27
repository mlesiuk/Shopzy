using FluentValidation;
using Shopzy.Application.Commands.RoleCommands;

namespace Shopzy.Application.Validators.UserRoleValidators;

public class AssignRoleCommandValidator : AbstractValidator<AssignRoleCommand>
{
    public AssignRoleCommandValidator()
    {
        RuleFor(ur => ur.User)
            .Must(u => u is not null && !string.IsNullOrEmpty(u.Name));

        RuleFor(ur => ur.Role)
            .Must(r => r is not null && !string.IsNullOrEmpty(r.Name));
    }
}
