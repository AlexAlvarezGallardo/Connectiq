namespace CustomerWorker.Domain.Commands;

public record CreateCustomerInput
{
    public required Customer Customer { get; init; }
}

public record CustomerValidated
{
    public required Customer Customer { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required bool IsValid { get; init; }
}

public record CustomerNotValidated
{
    public required Customer Customer { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required bool IsValid { get; init; }
    public required string NotValidatedMessage { get; init; }
}

public record CustomerCreate
{
    public required CustomerValidated CustomerValidated { get; init; }
    public required string EventId { get; init; }
    public required bool IsActive { get; init; }
}