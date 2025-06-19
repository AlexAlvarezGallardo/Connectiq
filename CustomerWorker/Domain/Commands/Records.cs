namespace CustomerWorker.Domain.Commands;

public record CustomerValidated
{
    public required Customers.Customer Customer { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required bool IsValid { get; init; }
}

public record CustomerCreate
{
    public required CustomerValidated CustomerValidated { get; init; }
    public required string EventId { get; init; }
    public required bool IsActive { get; init; }
}