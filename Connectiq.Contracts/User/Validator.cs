using FluentValidation;

namespace Connectiq.Contracts.User;

public class UserValidator : AbstractValidator<GrpcUsers.User>
{
    public UserValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.");

        RuleFor(user => user.PasswordHash)
            .NotEmpty().WithMessage("PasswordHash is required.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.");

        RuleFor(user => user.Roles)
            .NotNull().WithMessage("Roles cannot be null.");

        RuleFor(user => user.CreatedAt.ToDateTimeOffset())
            .LessThanOrEqualTo(DateTimeOffset.UtcNow).WithMessage("CreatedAt cannot be in the future.");

    }
}
