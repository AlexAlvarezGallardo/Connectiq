var builder = DistributedApplication.CreateBuilder(args);

var postgresServer = builder
    .AddPostgres("postgres")
    .WithPgWeb();

var postgresDb = postgresServer
    .AddDatabase("connectiq");

var rabbitMQ = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

var dbWorker = builder.AddProject<Projects.DatabaseWorker>("databaseworker")
    .WithReference(postgresDb)
    .WaitFor(postgresDb);

builder.AddProject<Projects.CustomerWorker>("customerworker")
    .WithReference(rabbitMQ)
    .WaitForCompletion(dbWorker)
    .WaitFor(rabbitMQ);

builder.AddProject<Projects.PersistenceWorker>("persistenceworker")
    .WithReference(rabbitMQ)
    .WithReference(postgresDb)
    .WaitForCompletion(dbWorker)
    .WaitFor(rabbitMQ);

builder.AddProject<Projects.Connectiq_API>("connectiq-api")
    .WithReference(rabbitMQ)
    .WaitFor(rabbitMQ);

builder.Build().Run();
