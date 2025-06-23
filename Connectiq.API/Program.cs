using Connectiq.ProjectDefaults.EventBus;
using Customers.Queries;
using static Customer.Queries.Service.CustomerQueryService;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddMutationGraphQLServices();
builder.Services.AddQueryGraphQLServices();

builder.Services.AddValidators<CustomerWorker.Worker>();
builder.Services.AddAutoMapper<CustomerWorker.Worker>();

builder.Services.AddMutationResponseFactory<CustomerValidated>();

builder.Services.AddQueryResponseFactory<GetCustomerResponse>();
builder.Services.AddQueryResponseFactory<GetCustomersResponse>();

builder.Services.Configure<EventBusOptions>(builder.Configuration.GetSection("EventBus"));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("rabbitmq"));

        cfg.Message<CustomerValidated>(x => x.SetEntityName("customer.events"));

        cfg.Publish<CustomerValidated>(p =>
        {
            p.ExchangeType = "topic";
        });
    });
});

builder.Services.AddGrpcClient<CustomerQueryServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["services:customerworker:customer-worker:0"]!);
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGraphQL();

app.Run();
