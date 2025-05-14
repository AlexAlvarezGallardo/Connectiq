using Connectiq.API.Application.Customer.Commands;
using Connectiq.Contracts.Customer;
using MediatR;

namespace Connectiq.API.GraphQL.Customer;

public class CustomerMutation
{
    public async Task<bool> CreateCustomer(CreateCustomerInput input, [Service] ISender sender)
    {
        var command = new CreateCustomerCommand(input);
        return await sender.Send(command);
    }
}