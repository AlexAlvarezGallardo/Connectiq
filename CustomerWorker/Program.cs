using Connectiq.ProjectDefaults.EventBus;
using MassTransit.Transports.Fabric;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddValidators<Program>();
builder.Services.AddAutoMapper<Program>();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddConnectiqProjectDefaults<CustomerEntity>();

builder.Services.AddGrpc();

builder.Services.Configure<EventBusOptions>(builder.Configuration.GetSection("EventBus"));

builder.Services.AddMassTransit(x => 
{ 
    x.AddConsumer<CreateCustomerEvent>();
    x.AddConsumer<UpdateCustomerEvent>();

    x.UsingRabbitMq((ctx, cfg) => 
    { 
        var options = ctx.GetRequiredService<IOptions<EventBusOptions>>().Value;

        cfg.Host(builder.Configuration.GetConnectionString("rabbitmq")); 

        cfg.Message<CustomerValidated>(configTopology => { 
            configTopology.SetEntityName(options.Exchange.Name); 
        });

        cfg.ReceiveEndpoint(options.Exchange.CreateCustomer.QueueName, e =>
        {
            e.ConfigureConsumeTopology = false;
            e.Bind(options.Exchange.Name, s => 
            { 
                s.RoutingKey = options.Exchange.CreateCustomer.RoutingKey;
                s.ExchangeType = ExchangeType.Topic.ToString().ToLower();
            });
            e.ConfigureConsumer<CreateCustomerEvent>(ctx);
        });

        cfg.ReceiveEndpoint(options.Exchange.UpdateCustomer.QueueName, e =>
        {
            e.ConfigureConsumeTopology = false;
            e.Bind(options.Exchange.Name, s =>
            {
                s.RoutingKey = options.Exchange.UpdateCustomer.RoutingKey;
                s.ExchangeType = ExchangeType.Topic.ToString().ToLower();
            });
            e.ConfigureConsumer<UpdateCustomerEvent>(ctx);
        });
    });
});

var app = builder.Build();

app.MapGrpcService<CustomerQuery>();

app.Run();
