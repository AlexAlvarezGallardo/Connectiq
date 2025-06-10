using PersistenceWorker;
using PersistenceWorker.Consumers.Customers;
using PersistenceWorker.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddInfrastructureServices<ConnectiqDbContext>(builder.Configuration, ConnectiqDbContext.SchemaName);
builder.Services.AddMessagingServices(builder.Configuration, x =>
{
    x.AddConsumer<CustomerValidatedEvent>();
});
builder.Services.AddRepositoryLayer();
builder.Services.AddContractsAutoMapper();

var host = builder.Build();
host.Run();
