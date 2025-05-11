using Connectiq.GrpcCustomers;
using DatabaseWorker.Domain.DbSeeder;
using DatabaseWorker.Extensions;
using DatabaseWorker.Services;
using Microsoft.EntityFrameworkCore;
using PersistenceWorker.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<DbSeederOptions>(builder.Configuration.GetSection("SeedData"));
builder.Services.AddTransient<IDbSeeder<ConnectiqDbContext>, DbSeeder<ConnectiqDbContext, Customer>>();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddMigrations<DbContext>(new Type[] { typeof(ConnectiqDbContext) });

var host = builder.Build();

host.Run();