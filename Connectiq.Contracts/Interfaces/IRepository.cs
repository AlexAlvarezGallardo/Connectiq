namespace Connectiq.Contracts.Interfaces;

public interface IRepository<TEntity> 
    where TEntity : class
{
    Task<bool> InsertAsync(TEntity entity);
}
