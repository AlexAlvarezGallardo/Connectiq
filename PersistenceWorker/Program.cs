using PersistenceWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMassTransitServices(builder.Configuration);

var host = builder.Build();
host.Run();
