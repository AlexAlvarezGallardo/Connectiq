using UserWorker;
using UserWorker.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddInfrastructureServices<UserDbContext>(builder.Configuration, UserDbContext.SchemaName);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
