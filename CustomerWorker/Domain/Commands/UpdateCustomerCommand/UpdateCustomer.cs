namespace CustomerWorker.Domain.Commands.UpdateCustomerCommand;

public record UpdateCustomer : CustomerBase;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomer>
{
    public UpdateCustomerValidator()
    {
        RuleFor(c => c)
            .SetValidator(new CustomerBaseValidator());
    }
}