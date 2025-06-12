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

builder.AddProject<Projects.CustomerWorker>("customerworker")
    .WithReference(rabbitMQ)
    .WithReference(customersDb)
    .WaitForCompletion(dbWorker)
    .WaitFor(rabbitMQ);

builder.AddProject<Projects.Connectiq_API>("connectiq-api")
    .WithReference(rabbitMQ)
    .WaitFor(rabbitMQ);

builder.Build().Run();
