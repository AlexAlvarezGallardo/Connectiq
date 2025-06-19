namespace DatabaseWorker.Domain.DbSeeder;

public interface IDbSeeder<in TContext> where TContext : DbContext
{
    Task SeedAsync(TContext context);
}
