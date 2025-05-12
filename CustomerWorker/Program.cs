using CustomerWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddValidators();
builder.Services.AddInfrastructureServices(builder.Configuration);

var host = builder.Build();
host.Run();
