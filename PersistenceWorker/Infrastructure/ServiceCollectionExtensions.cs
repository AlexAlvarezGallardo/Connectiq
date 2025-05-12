using MassTransit;
using Microsoft.EntityFrameworkCore;
using PersistenceWorker.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    =>  services
            .AddDbContext<ConnectiqDbContext>(options => {
                options.UseNpgsql(configuration.GetConnectionString("connectiq"));
               });

    public static IServiceCollection AddMassTransitServices(this IServiceCollection services, IConfiguration configuration)
        => services.AddMassTransit(busConfiguration =>
        {
            busConfiguration.SetKebabCaseEndpointNameFormatter();

            busConfiguration.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(configuration.GetConnectionString("rabbitmq"));
                configurator.ConfigureEndpoints(context);
            });
        });
}
