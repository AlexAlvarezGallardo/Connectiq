namespace CustomerWorker.Domain.Commands;

public class CustomerValidator : AbstractValidator<Customers.Customer>
{
    public CustomerValidator()
    {
        RuleFor(c => c.Details)
            .SetValidator(new CustomerDetailsValidator());

        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("El campo Id es obligatorio.");
    }
}

public class CustomerDetailsValidator : AbstractValidator<CustomerDetails>
{
    public CustomerDetailsValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio");

        RuleFor(c => c.Address)
            .NotEmpty()
            .WithMessage("La dirección es obligatoria");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("El email es obligatorio")
            .EmailAddress().WithMessage("Formato de email inválido");

        RuleFor(c => c.Phone)
            .NotEmpty().WithMessage("El teléfono es obligatorio")
            .Matches(@"^\+\d{2} \d{3} \d{3} \d{3}$")
            .WithMessage("Formato de teléfono inválido (+34 600 123 456)");
    }
}