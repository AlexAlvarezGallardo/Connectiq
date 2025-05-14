namespace Connectiq.Contracts.Customer;

public record CreateCustomerInput() 
{ 
    public required string Name { get; init; }
    public required string Phone { get; init; }
    public required string Email { get; init; }
    public required string Address { get; init; }
};

public record CustomerCreated() : CreateCustomerInput
{ 
    public string Id { get; init; }
};
