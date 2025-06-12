namespace CustomerWorker.Domain;

public class CustomerValidatedValidator : AbstractValidator<CustomerValidated>
{
    public CustomerValidatedValidator()
    {
        RuleFor(c => c.Customer.Name)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio");

        RuleFor(c => c.Customer.Address)
            .NotEmpty()
            .WithMessage("La dirección es obligatoria");

        RuleFor(c => c.Customer.Email)
            .NotEmpty().WithMessage("El email es obligatorio")
            .EmailAddress().WithMessage("Formato de email inválido");

        RuleFor(c => c.Customer.Phone)
            .NotEmpty().WithMessage("El teléfono es obligatorio")
            .Matches(@"^\+\d{2} \d{3} \d{3} \d{3}$")
            .WithMessage("Formato de teléfono inválido (+34 600 123 456)");

        RuleFor(c => c.CreatedAt)
            .NotEmpty().WithMessage("El campo CreatedAt es obligatorio.")
            .Must(date => date >= DateTimeOffset.UtcNow.AddDays(-1));

        RuleFor(c => c.IsValid)
            .NotEmpty()
            .WithMessage("El campo IsValid es obligatorio.");
    }
}

public class CustomerCreateValidator : AbstractValidator<CustomerCreate>
{
    public CustomerCreateValidator()
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
