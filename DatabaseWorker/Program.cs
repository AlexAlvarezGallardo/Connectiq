using Connectiq.Contracts.Customer;
using DatabaseWorker.Domain.DbSeeder;
using DatabaseWorker.Services;
using Microsoft.AspNetCore.Hosting;
using PersistenceWorker.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<DbSeederOptions>(builder.Configuration.GetSection("SeedData:DbSeederOptions"));
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddMigration<ConnectiqDbContext, DbSeeder<ConnectiqDbContext, CustomerEntity>>();

var host = builder.Build();

host.Run();