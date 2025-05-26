using Microsoft.EntityFrameworkCore;
using PersistenceWorker.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    => services
            .AddDbContext<ConnectiqDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("connectiq"));
            });
}
