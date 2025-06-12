using CustomerWorker.Domain;
using DatabaseWorker.Domain.DbSeeder;
using DatabaseWorker.Services;
using Microsoft.AspNetCore.Hosting;
using PersistenceWorker.Infrastructure;
using System.Text.Json;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<DbSeederOptions>(builder.Configuration.GetSection("SeedData:DbSeederOptions"));
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddMigration<ConnectiqDbContext, DbSeeder<ConnectiqDbContext, CustomerEntity>>();

builder.Services.AddSingleton(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

var host = builder.Build();

host.Run();