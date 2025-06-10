using Connectiq.Contracts.Customer;
using Connectiq.Contracts.User;
using DatabaseWorker.Domain.DbSeeder;
using DatabaseWorker.Services;
using Microsoft.AspNetCore.Hosting;
using PersistenceWorker.Infrastructure;
using System.Text.Json;
using UserWorker.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<DbSeederOptions>(builder.Configuration.GetSection("SeedData:DbSeederOptions"));

builder.Services.AddInfrastructureServices<ConnectiqDbContext>(builder.Configuration, ConnectiqDbContext.SchemaName);
builder.Services.AddInfrastructureServices<UserDbContext>(builder.Configuration, UserDbContext.SchemaName);

builder.Services.AddMigration<ConnectiqDbContext, DbSeeder<ConnectiqDbContext, CustomerEntity>>();
builder.Services.AddMigration<UserDbContext, DbSeeder<UserDbContext, UserEntity>>();

builder.Services.AddSingleton(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

var host = builder.Build();

host.Run();