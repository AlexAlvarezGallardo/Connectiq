var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddValidators<Program>();
builder.Services.AddAutoMapper<Program>();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddConnectiqProjectDefaults<CustomerEntity>();

builder.Services.AddGrpc();

builder.Services.AddMessagingServices(builder.Configuration, x =>
{
    x.AddConsumer<CustomerCreatedEvent>();
});

var app = builder.Build();

app.MapGrpcService<CustomerQuery>();

app.Run();
