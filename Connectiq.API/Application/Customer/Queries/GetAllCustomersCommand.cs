using Customers.Queries;
using static Customer.Queries.Service.CustomerQueryService;

namespace Connectiq.API.Application.Customer.Queries;

public record GetAllCustomersCommand(GetAllFiltersInput Input) : IRequest<GetCustomersResponse>;

public class GetAllCustomersCommandHandler(
    CustomerQueryServiceClient _customerClient) : IRequestHandler<GetAllCustomersCommand, GetCustomersResponse>
{
    public async Task<GetCustomersResponse> Handle(GetAllCustomersCommand request, CancellationToken cancellationToken)
        => await _customerClient.GetAllCustomersAsync(request.Input, cancellationToken: cancellationToken);
}
