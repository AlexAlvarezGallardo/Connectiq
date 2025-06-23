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

    public override async Task<GetCustomerResponse> GetCustomerById(GetCustomerByIdInput request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var id))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid customer ID format."));

        var customer = await _repository.GetEntityById<CustomerDto>(id);

        if (customer == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Customer not found."));

        return new GetCustomerResponse { CustomerDto = customer };

    }
}