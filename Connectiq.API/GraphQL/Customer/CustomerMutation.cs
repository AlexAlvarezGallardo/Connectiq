namespace Connectiq.API.GraphQL.Customer;

public class CustomerMutation
{
    public async Task<IMutationResponse<CustomerValidated>> CreateCustomer(CreateCustomerInput input, [Service] ISender sender)
    {
        var command = new CreateCustomerCommand(input);
        return await sender.Send(command);
    }

    public async Task<IMutationResponse<CustomerValidated>> UpdateCustomer(UpdateCustomerInput input, [Service] ISender sender)
    {
        var command = new UpdateCustomerCommand(input);
        return await sender.Send(command);
    }

    public async Task<IMutationResponse<CustomerValidated>> SoftDelete(DeleteCustomerInput input, [Service] ISender sender)
    {
        var command = new DeleteCustomerCommand(input);
        return await sender.Send(command);
    }
}