using FluentValidation;

namespace Connectiq.Contracts.User;

public class CreateUserInputValidator : AbstractValidator<CreateUserInput>
{
    public CreateUserInputValidator()
    {
        RuleFor(user => user.User.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.");

        RuleFor(user => user.User.Password)
            .NotEmpty().WithMessage("PasswordHash is required.");

        RuleFor(user => user.User.Roles)
            .NotNull().WithMessage("Roles cannot be null.");
    }
}

public class CreateUserValidator : AbstractValidator<CreateUser>
{
    public CreateUserValidator()
    {
        RuleFor(user => user.UserValidated.CreateUserInput)
            .SetValidator(new CreateUserInputValidator());

        RuleFor(user => user.EventId)
            .NotEmpty().WithMessage("EventId is required.");

        RuleFor(user => user.UserValidated.CreatedAt)
           .NotNull().WithMessage("CreatedAt is required")
           .LessThanOrEqualTo(DateTimeOffset.UtcNow).WithMessage("CreatedAt cannot be in the future");

        RuleFor(user => user.IsActive)
            .NotNull().WithMessage("IsActive status cannot be null.");
    }
}