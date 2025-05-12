using DatabaseWorker.Domain.DbSeeder;
using DatabaseWorker.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics;

namespace Microsoft.AspNetCore.Hosting;

internal static class MigrateDbContextExtensions
{
    static readonly string ActivitySourceName = "DbMigrations";
    static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    public static IServiceCollection AddMigration<TContext>(this IServiceCollection services)
        where TContext : DbContext
        => services.AddMigration<TContext>((_, _) => Task.CompletedTask);

    public static IServiceCollection AddMigration<TContext>(this IServiceCollection services, Func<TContext, IServiceProvider, Task> seeder)
        where TContext : DbContext
    {
        services.AddOpenTelemetry().WithTracing(tracing => tracing.AddSource(ActivitySourceName));
        return services.AddHostedService(sp => new Worker<TContext>(sp, seeder, sp.GetRequiredService<IHostApplicationLifetime>()));
    }

    public static IServiceCollection AddMigration<TContext, TDbSeeder>(this IServiceCollection services)
        where TContext : DbContext
        where TDbSeeder : class, IDbSeeder<TContext>
    {
        services.AddScoped<IDbSeeder<TContext>, TDbSeeder>();
        return services.AddMigration<TContext>((context, sp) => sp.GetRequiredService<IDbSeeder<TContext>>().SeedAsync(context));
    }

    internal static async Task MigrateDbContextAsync<TContext>(this IServiceProvider services, Func<TContext, IServiceProvider, Task> seeder, CancellationToken cancellationToken) 
        where TContext : DbContext
    {
        using var scope = services.CreateScope();
        var scopeServices = scope.ServiceProvider;
        var logger = scopeServices.GetRequiredService<ILogger<TContext>>();
        var context = scopeServices.GetService<TContext>();

        using var activity = ActivitySource.StartActivity($"Migration operation {typeof(TContext).Name}");

        try
        {
            logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

            var strategy = context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(() => InvokeSeederAsync(seeder, context, scopeServices));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);

            activity!.SetExceptionTags(ex);

            throw;
        }
    }

    static async Task InvokeSeederAsync<TContext>(Func<TContext, IServiceProvider, Task> seeder, TContext context, IServiceProvider services)
        where TContext : DbContext
    {
        using var activity = ActivitySource.StartActivity($"Migrating {typeof(TContext).Name}");

        try
        {
            await context.Database.MigrateAsync();
            await seeder(context, services);
        }
        catch (Exception ex)
        {
            activity!.SetExceptionTags(ex);
            throw;
        }
    }
}