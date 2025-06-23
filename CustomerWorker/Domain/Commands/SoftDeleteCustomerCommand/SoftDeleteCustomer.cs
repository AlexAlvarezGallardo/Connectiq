namespace CustomerWorker.Domain.Commands.SoftDeleteCustomerCommand;

public class SoftDeleteCustomer
{
    public Guid Id { get; set; }
}

public class SoftDeleteCustomerValidator : AbstractValidator<SoftDeleteCustomer>
{
    public SoftDeleteCustomerValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("El Id es obligatorio.");
    }
}