using CustomerWorker;
using CustomerWorker.Events;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddContractsValidators();
builder.Services.AddContractsAutoMapper();
builder.Services.AddMessagingServices(builder.Configuration, x =>
{
    x.AddConsumer<CustomerCreatedEvent>();
});

var host = builder.Build();
host.Run();
