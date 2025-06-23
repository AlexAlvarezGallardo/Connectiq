using MassTransit;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace MassTransit;

public static class MassTransitExtensions
{
    public static void ConfigureRabbitMqReceiveEndpoint<TMessage, TConsumer>(
       this IRabbitMqBusFactoryConfigurator cfg,
       IRegistrationContext context,
       IConfigurationSection queueConfig,
       string exchangeName)
       where TMessage : class
       where TConsumer : class, IConsumer<TMessage>
    {
        cfg.ReceiveEndpoint(queueConfig["QueueName"]!, e =>
        {
            e.ConfigureConsumeTopology = false;
            e.Bind(exchangeName, s =>
            {
                s.RoutingKey = queueConfig["RoutingKey"];
                s.ExchangeType = ExchangeType.Topic.ToString().ToLower();
            });
            e.ConfigureConsumer<TConsumer>(context);
        });
    }
}