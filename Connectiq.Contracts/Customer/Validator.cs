using FluentValidation;

namespace Connectiq.Contracts.Customer;

public class CustomerCreatedValidator : AbstractValidator<CustomerCreated>
{
    public CustomerCreatedValidator()
    {
        RuleFor(c => c.CustomerData.Name)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio");

        RuleFor(c => c.CustomerData.Address)
            .NotEmpty()
            .WithMessage("La dirección es obligatoria");

        RuleFor(c => c.CustomerData.Email)
            .NotEmpty().WithMessage("El email es obligatorio")
            .EmailAddress().WithMessage("Formato de email inválido");

        RuleFor(c => c.CustomerData.Phone)
            .NotEmpty().WithMessage("El teléfono es obligatorio")
            .Matches(@"^\+\d{2} \d{3} \d{3} \d{3}$")
            .WithMessage("Formato de teléfono inválido (+34 600 123 456)");
    }
}

public class CustomerValidatedValidator : AbstractValidator<CustomerValidated>
{
    public CustomerValidatedValidator()
    {
        RuleFor(c => c.CustomerCreated)
            .SetValidator(new CustomerCreatedValidator());

        RuleFor(c => c.CustomerCreated.EventId)
            .NotEmpty()
            .WithMessage("El EventId es obligatorio.");

        RuleFor(c => c.CustomerCreated.IsActive)
            .NotEmpty()
            .WithMessage("El campo IsActive es obligatorio.");

        RuleFor(c => c.CreatedAt)
            .NotEmpty().WithMessage("El campo CreatedAt es obligatorio.")
            .Must(date => date >= DateTimeOffset.UtcNow.AddDays(-1));
    }
}
