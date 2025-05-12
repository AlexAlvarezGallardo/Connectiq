using MassTransit;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) 
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

