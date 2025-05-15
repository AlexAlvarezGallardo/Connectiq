namespace CustomerWorker;

public class Worker(ILogger<Worker> _logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Customer Worker is running!");
        await Task.CompletedTask;
    }
}