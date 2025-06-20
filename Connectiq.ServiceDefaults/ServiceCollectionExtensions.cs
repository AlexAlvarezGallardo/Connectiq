using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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

    public static IServiceCollection AddValidators<T>(this IServiceCollection services)
    => services
        .AddValidatorsFromAssembly(Assembly.GetCallingAssembly())
        .AddValidatorsFromAssembly(typeof(T).Assembly);

    public static IServiceCollection AddAutoMapper<T>(this IServiceCollection services)
        => services.AddAutoMapper(x => { x.DisableConstructorMapping(); }, typeof(T).Assembly);
}

