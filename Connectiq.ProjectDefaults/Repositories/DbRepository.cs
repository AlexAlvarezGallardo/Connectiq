using Connectiq.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Connectiq.ProjectDefaults.Repositories;

internal class DbRepository<TContext, TEntity>(TContext _dbContext) : IRepository<TEntity>
    where TEntity : class
    where TContext : DbContext
{
    public async Task<bool> InsertAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        var result = await _dbContext.SaveChangesAsync();

        return result > 0;
    }
}