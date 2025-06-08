using Connectiq.GrpcUsers;

namespace Connectiq.Contracts.User;

public class UserEntity
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string Email { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public required ICollection<Role> Roles { get; set; } = Enumerable.Empty<Role>().ToList();
}
