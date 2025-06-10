using Connectiq.Contracts.User;
using Microsoft.EntityFrameworkCore;

namespace UserWorker.Infrastructure;

public class UserDbContext(DbContextOptions<UserDbContext> options)
    : DbContext(options)
{
    public static string SchemaName { get; } = "users";

    public DbSet<UserEntity> Users => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
