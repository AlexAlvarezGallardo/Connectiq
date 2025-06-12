namespace CustomerWorker.Repository;

public class DbRepository<TEntity>(CustomerDbContext _connectiqDbContext) : IRepository<TEntity>
    where TEntity : class
{
    public async Task<bool> InsertAsync(TEntity entity)
    {
        await _connectiqDbContext.Set<TEntity>().AddAsync(entity);
        var result = await _connectiqDbContext.SaveChangesAsync();

        return result > 0;
    }
}
