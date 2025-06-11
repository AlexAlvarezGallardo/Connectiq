using Connectiq.GrpcUsers;

namespace Connectiq.Contracts.User;

public class UserEntity
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required bool IsActive { get; set; }
    public required Guid EventId { get; set; }
    public required Role[] Roles { get; set; } = [];
}
