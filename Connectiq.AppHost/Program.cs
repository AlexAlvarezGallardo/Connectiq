using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgresServer = builder
    .AddPostgres("postgres")
    .WithPgWeb();

var customersDb = postgresServer
    .AddDatabase("customers");

var rabbitMQ = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

var dbWorker = builder.AddProject<Projects.DatabaseWorker>("databaseworker")
    .WithReference(customersDb)
    .WaitFor(customersDb);

var customerWorker = builder.AddProject<Projects.CustomerWorker>("customerworker")
    .WithEndpoint(name: "customer-worker", scheme: "https")
    .WithReference(rabbitMQ)
    .WithReference(customersDb)
    .WaitForCompletion(dbWorker)
    .WaitFor(rabbitMQ);

builder.AddProject<Projects.Connectiq_API>("connectiq-api")
    .WithReference(rabbitMQ)
    .WithReference(customerWorker)
    .WaitFor(customerWorker)
    .WaitFor(rabbitMQ);

builder.Build().Run();
