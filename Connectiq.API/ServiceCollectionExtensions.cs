using Connectiq.API.GraphQL.Types.Customer;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddMutationGraphQLServices(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddMutationType<CustomerMutation>()
            .AddType<CustomerFiltersInputType>()
            .AddType<CustomerValidatedResultType>();

        return services;
    }

    public static IServiceCollection AddQueryGraphQLServices(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddQueryType<CustomerQuery>()
            .AddType<CustomerType>()
            .AddType<CustomerQueryResultType>()
            .AddType<CustomersType>()
            .AddType<CustomersQueryResultType>();

        return services;
    }
}
