using System.Linq.Expressions;

namespace Connectiq.ProjectDefaults;

public interface IRepository<TEntity> where TEntity : class
{
    Task<bool> InsertAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TEntity entity);
    Task<ICollection<TOutput>> GetAllAsync<TOutput>(int? page = null, int? pageSize = null, Expression<Func<TEntity, bool>>? filter = null);
}
