using Connectiq.ProjectDefaults.Response.Query;
using Connectiq.ProjectDefaults.Response.Query.Factory;
using Customers.Queries;
using static Customer.Queries.Service.CustomerQueryService;

namespace Connectiq.API.Application.Customer.Queries;

public record GetCustomerByIdCommand(GetCustomerByIdInput Input) : IRequest<IQueryResponse<GetCustomerResponse>>;

public class GetCustomerByIdCommandHandler(
    CustomerQueryServiceClient _customerClient,
    IQueryResultFactory _queryResultFactory)
    : IRequestHandler<GetCustomerByIdCommand, IQueryResponse<GetCustomerResponse>>
{
    public async Task<IQueryResponse<GetCustomerResponse>> Handle(GetCustomerByIdCommand request, CancellationToken cancellationToken)
    {
        var response = await _customerClient.GetCustomerByIdAsync(request.Input, cancellationToken: cancellationToken);
        return _queryResultFactory.Ok(response);
    }
}