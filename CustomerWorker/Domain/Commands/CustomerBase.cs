namespace CustomerWorker.Domain.Commands;

public record CustomerBase
{
    public required CustomerValidated CustomerValidated { get; init; }
    public required string EventId { get; init; }
    public required bool IsActive { get; init; }
}

public class CustomerBaseValidator : AbstractValidator<CustomerBase>
{
    public CustomerBaseValidator()
    {
        RuleFor(c => c.CustomerValidated)
            .SetValidator(new CustomerValidatedValidator());

        RuleFor(c => c.EventId)
            .NotEmpty()
            .WithMessage("El EventId es obligatorio.");

        RuleFor(c => c.IsActive)
            .NotEmpty()
            .WithMessage("El campo IsActive es obligatorio.");
    }
}