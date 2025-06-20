namespace CustomerWorker.Domain.Commands.CreateCustomerCommand;

public record CreateCustomer
{
    public required CustomerValidated CustomerValidated { get; init; }
    public required string EventId { get; init; }
    public required bool IsActive { get; init; }
}

public class CreateCustomerValidator : AbstractValidator<CreateCustomer>
{
    public CreateCustomerValidator()
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

