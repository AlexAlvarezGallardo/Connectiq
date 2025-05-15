namespace Connectiq.Contracts.Customer;

public record CustomerData
{
    public required string Name { get; init; }
    public required string Phone { get; init; }
    public required string Email { get; init; }
    public required string Address { get; init; }
}

public record CreateCustomerInput
{
    public required CustomerData Customer { get; init; }
}

public record CustomerCreated
{
    public required CustomerData CustomerData { get; init; }
    public required string EventId { get; init; }
    public required bool IsActive { get; init; }
}

public record CustomerValidated
{
    public required CustomerCreated CustomerCreated { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required bool IsValid { get; init; }
}

public record CustomerNotValidated
{
    public required CustomerCreated CustomerCreated { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required bool IsValid { get; init; }
    public required string NotValidatedMessage { get; init; }
}
