using LinqKit;
using LinqKit.Core;

namespace CustomerWorker.Repository;

public class DbRepository<TEntity>(
    CustomerDbContext _connectiqDbContext,
    IMapper _mapper) : IRepository<TEntity>
    where TEntity : class
{
    public async Task<bool> InsertAsync(TEntity entity)
    {
        await _connectiqDbContext.Set<TEntity>().AddAsync(entity);
        var result = await _connectiqDbContext.SaveChangesAsync();

        return result > 0;
    }

    public async Task<ICollection<TOutput>> GetAllAsync<TOutput>(
        int? page = null, 
        int? pageSize = null,
        Expression<Func<TEntity, bool>>? filter = null)
    {
        var query = _connectiqDbContext.Set<TEntity>().AsExpandableEFCore();

        if (filter is not null)
            query = query.Where(filter);

        if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

        var entities = await query.ToListAsync();

        return _mapper.Map<ICollection<TOutput>>(entities);
    }
}
