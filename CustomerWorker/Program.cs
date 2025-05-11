using CustomerWorker;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddMassTransit(busConfiguration =>
{
    busConfiguration.SetKebabCaseEndpointNameFormatter();

    busConfiguration.UsingRabbitMq((context, configurator) => 
    {
        configurator.Host(builder.Configuration.GetConnectionString("rabbitmq"));
        configurator.ConfigureEndpoints(context);
    });
});

var host = builder.Build();
host.Run();
