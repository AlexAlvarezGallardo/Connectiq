using Connectiq.ProjectDefaults.Response.Query;
using Connectiq.ProjectDefaults.Response.Query.Factory;
using Customers.Queries;
using static Customer.Queries.Service.CustomerQueryService;

namespace Connectiq.API.Application.Customer.Queries;

public record GetAllCustomersCommand(
    GetAllFiltersInput Input) : IRequest<IQueryResponse<GetCustomersResponse>>;

public class GetAllCustomersCommandHandler(
    CustomerQueryServiceClient _customerClient,
    IQueryResultFactory _queryResultFactory) : IRequestHandler<GetAllCustomersCommand, IQueryResponse<GetCustomersResponse>>
{
    public async Task<IQueryResponse<GetCustomersResponse>> Handle(GetAllCustomersCommand request, CancellationToken cancellationToken)
    {
        var response = await _customerClient.GetAllCustomersAsync(request.Input, cancellationToken: cancellationToken);
        return _queryResultFactory.Ok(response);
    }
}
