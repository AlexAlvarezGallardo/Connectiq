using Connectiq.ProjectDefaults.LinqExtensions;
using static Customer.Queries.Service.CustomerQueryService;

namespace CustomerWorker.Service;

public class CustomerQuery(IRepository<CustomerEntity> _repository, ILinqExtensions<CustomerEntity> _linqExtensions) : CustomerQueryServiceBase
{
    public override async Task<GetCustomersResponse> GetAllCustomers(GetAllFiltersInput request, ServerCallContext context)
    {
        var filter = _linqExtensions.Build(request.Filters);

        var customers = await _repository.GetAllAsync<CustomerDto>(request.Page, request.PageSize, filter);

        return new GetCustomersResponse
        {
            Customers = { customers },
            TotalCount = customers.Count
        };
    }
}