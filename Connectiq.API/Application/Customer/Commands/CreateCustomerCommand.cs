using Connectiq.API.GraphQL.Customer;
using Connectiq.Contracts.Customer;
using MassTransit;
using MediatR;

namespace Connectiq.API.Application.Customer.Commands;

public record CreateCustomerCommand(CreateCustomerInput input) : IRequest<Guid>;

public class CreateCustomerCommandHandler(IPublishEndpoint _publisher) : IRequestHandler<CreateCustomerCommand, Guid>
{
    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();

        //TODO: Refactor this to use a mapper
        var customerCreated = new CustomerCreated(
            id,
            request.input.Name,
            request.input.Address,
            request.input.Phone,
            request.input.Email
        );

        await _publisher.Publish(customerCreated, cancellationToken);
        return id;
    }
}

