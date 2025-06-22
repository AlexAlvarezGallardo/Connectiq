namespace CustomerWorker.Domain.Commands.CreateCustomerCommand;

public record CreateCustomer : CustomerBase;

public class CreateCustomerValidator : AbstractValidator<CreateCustomer>
{
    public CreateCustomerValidator()
    {
        RuleFor(c => c)
            .SetValidator(new CustomerBaseValidator());
    }
}

