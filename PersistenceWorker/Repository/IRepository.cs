namespace PersistenceWorker.Repository;

public interface IRepository<TEntity> where TEntity : class
{
    Task<bool> InsertAsync(TEntity entity);
}
