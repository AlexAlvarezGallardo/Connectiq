using Connectiq.API.GraphQL.Customer;
using Connectiq.Contracts.Customer;
using FluentValidation;
using MassTransit;
using MediatR;

namespace Connectiq.API.Application.Customer.Commands;

public record CreateCustomerCommand(CreateCustomerInput input) : IRequest<bool>;

public class CreateCustomerCommandHandler(
    IPublishEndpoint _publisher, 
    IValidator<CustomerCreated> _validator,
    CustomerMapper _mapper) : IRequestHandler<CreateCustomerCommand, bool>
{
    public async Task<bool> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerCreated = _mapper.ToEvent(request.input);
        var validatorResult = await _validator.ValidateAsync(customerCreated);

        if(!validatorResult.IsValid)
            return false;

        await _publisher.Publish(customerCreated, cancellationToken);
        return true;
    }
}

