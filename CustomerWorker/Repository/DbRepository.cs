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

    public async Task<ICollection<TOutput>> GetAllAsync<TOutput>()
    {
        var entities = await _connectiqDbContext.Set<TEntity>().ToListAsync();
        return _mapper.Map<ICollection<TOutput>>(entities);
    }
}
