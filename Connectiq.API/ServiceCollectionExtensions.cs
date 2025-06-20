using Connectiq.API.GraphQL.Types.Customer;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddGraphQLServices(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddQueryType<CustomerQuery>()
            .AddMutationType<CustomerMutation>()
            .AddType<CustomerFiltersInputType>()
            .AddType<CustomerValidatedResultType>();

        return services;
    }
}
