using DatabaseWorker.Domain.DbSeeder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using PersistenceWorker;
using System.Diagnostics;

namespace DatabaseWorker.Extensions;

internal static class MigrateDbContextExtensions
{
    public static readonly ActivitySource ActivitySource = new("DbMigrations");

    public static IServiceCollection AddMigrations<TContext>(this IServiceCollection services, params Type[] dbContexts)
        where TContext : DbContext
    {
        return services.AddMigration<TContext>(dbContexts: dbContexts);
    }

    public static IServiceCollection AddMigration<TContext>(this IServiceCollection services, Func<TContext, IServiceProvider, Task> seeder = default, params Type[] dbContexts)
        where TContext : DbContext
    {
        services.AddOpenTelemetry().WithTracing(tracing => tracing.AddSource("DbMigrations"));
        return services.AddHostedService(serviceProvider => new Worker<TContext>(serviceProvider, seeder, dbContexts));
    }

    public static IServiceCollection AddMigration<TContext, TDbSeeder>(this IServiceCollection services)
        where TContext : DbContext
        where TDbSeeder : class, IDbSeeder<TContext>
    {
        services.AddScoped<IDbSeeder<TContext>, TDbSeeder>();
        return services.AddMigration<TContext>((context, serviceProvider) => serviceProvider.GetRequiredService<IDbSeeder<TContext>>().SeedAsync(context));
    }

    public static async Task MigrateDbContextAsync<TContext>(
        this IServiceProvider services,
        Func<TContext, IServiceProvider, Task> seeder,
        CancellationToken cancellationToken,
        params Type[] dbContexts)
        where TContext : DbContext
    {
        using var scope = services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        foreach (var dbContext in dbContexts)
        {
            var logger = loggerFactory.CreateLogger(dbContext);
            var context = serviceProvider.GetService(dbContext) as DbContext;
            var dbCreator = context?.GetService<IRelationalDatabaseCreator>();

            using var activity = ActivitySource.StartActivity($"Migration operation {dbContext.Name}");

            try
            {
                logger.LogInformation($"Migrating database associated with contextName {dbContext.Name}");

                var strategy = context?.Database.CreateExecutionStrategy();

                if (!await dbCreator!.ExistsAsync(cancellationToken))
                    await dbCreator.CreateAsync(cancellationToken);

                if (strategy is not null)
                    await strategy.ExecuteAsync(() => context!.Database.MigrateAsync(cancellationToken));

                if (seeder is not null)
                    await seeder((TContext)context, serviceProvider);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", dbContext.Name);
                activity?.SetExceptionTags(ex);
                throw;
            }
        }

        serviceProvider.GetService<IHostApplicationLifetime>()!.StopApplication();
    }
}
