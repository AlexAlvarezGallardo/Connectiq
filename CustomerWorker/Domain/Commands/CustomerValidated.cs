namespace CustomerWorker.Domain.Commands;

public record CustomerValidated
{
    public required Customers.Customer Customer { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required bool IsValid { get; init; }
}

public class CustomerValidatedValidator : AbstractValidator<CustomerValidated>
{
    public CustomerValidatedValidator()
    {
        RuleFor(c => c.Customer)
            .SetValidator(new CustomerValidator());

        RuleFor(c => c.CreatedAt)
            .NotEmpty()
            .WithMessage("El campo CreatedAt es obligatorio.")
            .Must(date => date <= DateTimeOffset.UtcNow);

        RuleFor(c => c.IsValid)
            .NotEmpty()
            .WithMessage("El campo IsValid es obligatorio.");
    }
}