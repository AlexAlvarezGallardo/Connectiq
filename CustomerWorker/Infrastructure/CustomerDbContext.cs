namespace CustomerWorker.Infrastructure;

public class CustomerDbContext(DbContextOptions<CustomerDbContext> options)
    : DbContext(options)
{
    public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
