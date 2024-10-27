using FluentValidation;
using Shopzy.Application.Commands.CategoryCommands;

namespace Shopzy.Application.Validators.CategoryValidators;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(entity => entity.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);
    }
}
