using Connectiq.API.Application.Customer.Queries;
using Connectiq.ProjectDefaults.Response.Query;
using Customers.Queries;

namespace Connectiq.API.GraphQL.Customer;

public class CustomerQuery
{
    public async Task<IQueryResponse<GetCustomersResponse>> GetAllCustomersAsync(
        GetAllFiltersInput input,
        [Service] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetAllCustomersCommand(input);
        return await sender.Send(query, cancellationToken);
    }

    public async Task<IQueryResponse<GetCustomerResponse>> GetCustomerByIdAsync(
        GetCustomerByIdInput input,
        [Service] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetCustomerByIdCommand(input);
        return await sender.Send(query, cancellationToken);
    }
}
