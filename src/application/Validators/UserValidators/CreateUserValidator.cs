using FluentValidation;
using Shopzy.Application.Commands.UserCommands;

namespace Shopzy.Application.Validators.UserValidators;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(entity => entity.Username)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);

        RuleFor(entity => entity.Email)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255)
            .EmailAddress();

        RuleFor(entity => entity.Password)
            .NotNull()
            .NotEmpty()
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")
            .MaximumLength(64).WithMessage("Your password length must not exceed 64.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[\!\@\#\$\%\^\&\*\(\)\-\=\+]+").WithMessage("Your password must contain at least one special character.");
    }
}
