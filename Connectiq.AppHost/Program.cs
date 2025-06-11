var builder = DistributedApplication.CreateBuilder(args);

var postgresServer = builder
    .AddPostgres("postgres")
    .WithPgWeb();

var connectiqDb = postgresServer
    .AddDatabase("connectiq");

var usersDb = postgresServer
    .AddDatabase("users");

var rabbitMQ = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

var dbWorker = builder.AddProject<Projects.DatabaseWorker>("databaseworker")
    .WithReference(connectiqDb)
    .WithReference(usersDb)
    .WaitFor(connectiqDb)
    .WaitFor(usersDb);

builder.AddProject<Projects.CustomerWorker>("customerworker")
    .WithReference(rabbitMQ)
    .WaitForCompletion(dbWorker)
    .WaitFor(rabbitMQ);

builder.AddProject<Projects.PersistenceWorker>("persistenceworker")
    .WithReference(rabbitMQ)
    .WithReference(connectiqDb)
    .WaitForCompletion(dbWorker)
    .WaitFor(rabbitMQ);

builder.AddProject<Projects.Connectiq_API>("connectiq-api")
    .WithReference(rabbitMQ)
    .WaitFor(rabbitMQ);

builder.AddProject<Projects.UserWorker>("userworker")
    .WithReference(usersDb)
    .WaitForCompletion(dbWorker)
    .WaitFor(usersDb);

builder.Build().Run();
