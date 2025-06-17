using Connectiq.ProjectDefaults.LinqExtensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConnectiqProjectDefaults<TEntity>(this IServiceCollection services)
        where TEntity : class 
    {
        return services
            .AddScoped<ILinqExtensions<TEntity>, LinqExtensions<TEntity>>();
    }
}
