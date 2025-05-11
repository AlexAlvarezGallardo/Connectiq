using Microsoft.EntityFrameworkCore;
using PersistenceWorker.Infrastructure.Entities;

namespace PersistenceWorker.Infrastructure;

public class ConnectiqDbContext(DbContextOptions<ConnectiqDbContext> options)
    : DbContext(options)
{
    public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConnectiqDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
