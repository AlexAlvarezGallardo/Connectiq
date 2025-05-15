using PersistenceWorker.Infrastructure;

namespace PersistenceWorker.Repository;

internal class DbRepository<TEntity>(ConnectiqDbContext _connectiqDbContext) : IRepository<TEntity>
    where TEntity : class
{
    public async Task<bool> InsertAsync(TEntity entity)
    {
        await _connectiqDbContext.Set<TEntity>().AddAsync(entity);
        var result = await _connectiqDbContext.SaveChangesAsync();

        return result > 0;
    }
}
