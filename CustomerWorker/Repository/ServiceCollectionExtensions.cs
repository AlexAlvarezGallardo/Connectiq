namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    => services
            .AddScoped<IRepository<CustomerEntity>, DbRepository<CustomerEntity>>();
}
