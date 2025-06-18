using static Customer.Queries.Service.CustomerQueryService;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddGraphQLServices();
builder.Services.AddValidators<CustomerWorker.Worker>();
builder.Services.AddAutoMapper<CustomerWorker.Worker>();
builder.Services.AddMessagingServices(builder.Configuration);

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
