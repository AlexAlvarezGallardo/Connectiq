using CustomerWorker.Domain;
using PersistenceWorker.Repository;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositoryLayer(this IServiceCollection services)
    => services
            .AddScoped<IRepository<CustomerEntity>, DbRepository<CustomerEntity>>();
}
