namespace Connectiq.Contracts.Customer;

public record CustomerCreated(
    Guid Id,
    string Name,
    string Address,
    string Phone,
    string Email
);