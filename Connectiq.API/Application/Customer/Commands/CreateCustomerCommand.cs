namespace Connectiq.API.Application.Customer.Commands;

public record CreateCustomerCommand(CreateCustomerInput Input) : IRequest<bool>;

public class CreateCustomerCommandHandler(
    IPublishEndpoint _publisher,
    IValidator<CustomerValidated> _validator,
    IMapper _mapper) : IRequestHandler<CreateCustomerCommand, bool>
{
    public async Task<bool> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerValidated = _mapper.Map<CustomerValidated>(request.Input);

        var validatorResult = await _validator.ValidateAsync(customerValidated, cancellationToken);

        if (!validatorResult.IsValid)
            return false;

        await _publisher.Publish(customerValidated, cancellationToken);

        return true;
    }
}

