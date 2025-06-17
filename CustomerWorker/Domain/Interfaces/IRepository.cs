using System.Linq.Expressions;

namespace CustomerWorker.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<bool> InsertAsync(TEntity entity);
    Task<ICollection<TOutput>> GetAllAsync<TOutput>(int? page = null, int? pageSize = null, Expression<Func<TEntity, bool>>? filter = null);
}
