namespace PersistenceWorker.Infrastructure.Entities;

public class CustomerEntity
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}