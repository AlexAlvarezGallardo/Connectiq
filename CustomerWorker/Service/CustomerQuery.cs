using static Customer.Queries.Service.CustomerQueryService;

namespace CustomerWorker.Service;

public class CustomerQuery(IRepository<CustomerEntity> _repository) : CustomerQueryServiceBase
{ 
    public override async Task<GetCustomersResponse> GetCustomers(GetCustomersInput request, ServerCallContext context)
    {
        var customers = await _repository.GetAllAsync<CustomerDto>();

        return new GetCustomersResponse
        {
            Customers = { customers },
            TotalCount = customers.Count
        };
    }
}