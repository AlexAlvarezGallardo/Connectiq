namespace Connectiq.API.GraphQL.Customer;

public class CustomerMutation
{
    public async Task<IMutationResponse<CustomerValidated>> CreateCustomer(CreateCustomerInput input, [Service] ISender sender)
    {
        var command = new CreateCustomerCommand(input);
        return await sender.Send(command);
    }
}