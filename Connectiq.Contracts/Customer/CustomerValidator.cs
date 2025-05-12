using GrpcCustomer = Connectiq.GrpcCustomers.Customer;
using FluentValidation;

namespace CustomerWorker.Domain.Customer.Validators;

public class CustomerValidator : AbstractValidator<GrpcCustomer>
{
    public CustomerValidator()
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
            .Matches(@"^\+34 \d{3} \d{3} \d{3}$")
            .WithMessage("Formato de teléfono inválido (+34 600 123 456)");
    }
}
