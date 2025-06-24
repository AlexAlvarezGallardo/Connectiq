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

    public async Task<IMutationResponse<CustomerValidated>> SoftDelete(SoftDeleteCustomerInput input, [Service] ISender sender)
    {
        var command = new SoftDeleteCustomerCommand(input);
        return await sender.Send(command);
    }
}