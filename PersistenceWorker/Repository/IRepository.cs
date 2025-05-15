namespace PersistenceWorker.Repository;

internal interface IRepository<TEntity> where TEntity : class
{
    Task<bool> InsertAsync(TEntity entity);
}
