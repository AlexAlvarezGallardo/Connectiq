using Connectiq.Contracts.Customer;
using Connectiq.GrpcCustomers;
using MassTransit;

namespace CustomerWorker;

public class Worker(ILogger<Worker> _logger, IBus _bus) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var endpoint = await _bus.GetSendEndpoint(new Uri("queue:customer-service"));

        var customer = new Customer
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Juan Cliente",
            Address = "Calle Falsa 123",
            Phone = "+34 600 123 456",
            Email = "juan@example.com"
        };

        var customerCreated = new CustomerCreated(
            Guid.Parse(customer.Id),
            customer.Name,
            customer.Address,
            customer.Phone,
            customer.Email
        );

        await endpoint.Send(customer, stoppingToken);
        _logger.LogInformation("Evento enviado: {Name}", customer.Name);
    }
}