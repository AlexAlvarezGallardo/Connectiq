namespace Connectiq.ProjectDefaults.EventBus;

public class EventBusOptions
{
    public ExchangeSettings Exchange { get; set; } = new();

    public class ExchangeSettings
    {
        public string Name { get; set; } = string.Empty;
        public EventSettings CreateCustomer { get; set; } = new();
        public EventSettings UpdateCustomer { get; set; } = new();
    }

    public class EventSettings
    {
        public string ExchangeName { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
        public string RoutingKey { get; set; } = string.Empty;
    }
}
