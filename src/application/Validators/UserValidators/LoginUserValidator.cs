using FluentValidation;
using Shopzy.Application.Commands.UserCommands;

namespace Shopzy.Application.Validators.UserValidators;

public class LoginUserValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserValidator()
    {
        RuleFor(entity => entity.Username)
            .NotNull()
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(255);

        RuleFor(entity => entity.Password)
            .NotNull()
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(255);
    }
}
