using DatabaseWorker.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.AspNetCore.Hosting;

public class Worker<TContext>(
    IServiceProvider _serviceProvider,
    Func<TContext, IServiceProvider, Task> seeder,
    params Type[] dbContexts)
    : BackgroundService where TContext : DbContext
{

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _serviceProvider.MigrateDbContextAsync(seeder, stoppingToken, dbContexts);
    }
}
