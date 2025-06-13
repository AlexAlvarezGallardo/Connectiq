namespace CustomerWorker.Domain;

public class CustomerEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsActive { get; set; }
}