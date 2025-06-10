namespace Connectiq.Contracts.User;

public record CreateUserInput
{
    public required GrpcUsers.User User { get; init; }
}

public record UserValidated
{
    public required CreateUserInput CreateUserInput { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required bool IsValid { get; init; }
}

public record UserNotValidated
{
    public required CreateUserInput CreateUserInput { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required bool IsValid { get; init; }
    public required string NotValidatedMessage { get; init; }
}

public record CreateUser
{
    public required UserValidated UserValidated { get; init; }
    public required string EventId { get; init; }
    public required bool IsActive { get; init; }
}

