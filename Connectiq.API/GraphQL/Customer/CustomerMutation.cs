using Connectiq.API.Application.Customer.Commands;
using MediatR;

namespace Connectiq.API.GraphQL.Customer;

public class CustomerMutation
{
    public async Task<Guid> CreateCustomer(CreateCustomerInput input, [Service] ISender sender)
    {
        var command = new CreateCustomerCommand(input);
        return await sender.Send(command);
    }
}

public record CreateCustomerInput(string Name, string Address, string Phone, string Email);