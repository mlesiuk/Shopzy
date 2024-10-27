using FluentValidation;
using Shopzy.Domain.Entities;

namespace Shopzy.Application.Validators.RoleValidators;

public class CreateRoleValidator : AbstractValidator<Role>
{
    public CreateRoleValidator()
    {
        RuleFor(entity => entity.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);
    }
}
