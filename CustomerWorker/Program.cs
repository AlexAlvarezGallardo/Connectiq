var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddValidators<Program>();
builder.Services.AddAutoMapper<Program>();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddRepositories();

builder.Services.AddMessagingServices(builder.Configuration, x =>
{
    x.AddConsumer<CustomerCreatedEvent>();
});

var host = builder.Build();
host.Run();
