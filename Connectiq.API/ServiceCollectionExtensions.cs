using Connectiq.API.InputType;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddGraphQLServices(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddQueryType<CustomerQuery>()
            .AddMutationType<CustomerMutation>()
            .AddType<CustomerFiltersInputType>();

        return services;
    }
}
