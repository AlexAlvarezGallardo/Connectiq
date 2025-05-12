using Microsoft.EntityFrameworkCore;

namespace Microsoft.AspNetCore.Hosting;

public class Worker<TContext>(
    IServiceProvider _serviceProvider,
    Func<TContext, IServiceProvider, Task> _seeder,
    IHostApplicationLifetime _host)
    : BackgroundService where TContext : DbContext
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _serviceProvider.MigrateDbContextAsync(_seeder, stoppingToken);
        _host.StopApplication();
    }
}
