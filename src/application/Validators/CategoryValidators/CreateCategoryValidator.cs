using FluentValidation;
using Shopzy.Application.Commands.CategoryCommands;

namespace Shopzy.Application.Validators.CategoryValidators;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(entity => entity.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);
    }
}
