using Connectiq.API.Application.Customer.Queries;
using Customers.Queries;

namespace Connectiq.API.GraphQL.Customer;

public class CustomerQuery
{
    public async Task<GetCustomersResponse> GetAllCustomersAsync(
        GetAllFiltersInput input,
        [Service] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetAllCustomersCommand(input);
        return await sender.Send(query, cancellationToken);
    }
}
