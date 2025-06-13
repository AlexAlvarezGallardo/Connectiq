using CustomerWorker.Domain;
using CustomerWorker.Infrastructure;
using DatabaseWorker.Domain.DbSeeder;
using DatabaseWorker.Services;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<DbSeederOptions>(builder.Configuration.GetSection("SeedData:DbSeederOptions"));
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddMigration<CustomerDbContext, DbSeeder<CustomerDbContext, CustomerEntity>>();

builder.Services.AddSingleton(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

var host = builder.Build();

host.Run();