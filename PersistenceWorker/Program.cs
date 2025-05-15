using PersistenceWorker;
using PersistenceWorker.Consumers.Customers;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMessagingServices(builder.Configuration, x => 
{
    x.AddConsumer<CustomerValidatedEvent>();
});
builder.Services.AddRepositoryLayer();
builder.Services.AddContractsAutoMapper();

var host = builder.Build();
host.Run();
