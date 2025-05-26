using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessagingServices(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? consumers = null)
            => services.AddMassTransit(x =>
            {
                consumers?.Invoke(x);

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("rabbitmq"));
                    cfg.ConfigureEndpoints(ctx);
                });
            });
}

