using Connectiq.ProjectDefaults.EventBus;
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
    x.AddConsumer<SoftDeleteCustomerEvent>();

    x.AddConfigureEndpointsCallback((name, cfg) =>
    {
        if (cfg is IRabbitMqReceiveEndpointConfigurator rmq)
            rmq.SetQuorumQueue(3);
    });

    x.UsingRabbitMq((ctx, cfg) =>
    {
        var options = ctx.GetRequiredService<IOptions<EventBusOptions>>().Value;

        cfg.Host(builder.Configuration.GetConnectionString("rabbitmq"));

        cfg.Message<CustomerValidated>(configTopology => { configTopology.SetEntityName(options.Exchange.Name); });

        cfg.ConfigureRabbitMqReceiveEndpoint<CustomerValidated, CreateCustomerEvent>(
            ctx,
            builder.Configuration.GetSection("EventBus:Exchange:CreateCustomer"),
            options.Exchange.Name);

        cfg.ConfigureRabbitMqReceiveEndpoint<CustomerValidated, UpdateCustomerEvent>(
            ctx,
            builder.Configuration.GetSection("EventBus:Exchange:UpdateCustomer"),
            options.Exchange.Name);

        cfg.ConfigureRabbitMqReceiveEndpoint<CustomerValidated, SoftDeleteCustomerEvent>(
            ctx,
            builder.Configuration.GetSection("EventBus:Exchange:SoftDeleteCustomer"),
            options.Exchange.Name);
    });
});

var app = builder.Build();

app.MapGrpcService<CustomerQuery>();

app.Run();
