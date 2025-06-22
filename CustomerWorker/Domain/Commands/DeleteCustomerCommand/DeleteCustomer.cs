namespace CustomerWorker.Domain.Commands.DeleteCustomerCommand;

public class DeleteCustomer
{
    public Guid Id { get; set; }
}

public class DeleteCustomerValidator : AbstractValidator<DeleteCustomer>
{
    public DeleteCustomerValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("El Id es obligatorio.");
    }
}