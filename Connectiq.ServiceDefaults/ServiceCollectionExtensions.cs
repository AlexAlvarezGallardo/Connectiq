using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    => services.AddValidatorsFromAssembly(typeof(DependencyInjection.ServiceCollectionExtensions).Assembly);

    public static IServiceCollection AddMessagingServices(this IServiceCollection services, IConfiguration configuration)
        => services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("rabbitmq"));
                cfg.ConfigureEndpoints(ctx);
            });
        });
}

