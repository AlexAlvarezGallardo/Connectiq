using System.Linq.Expressions;

namespace Connectiq.ProjectDefaults.LinqExtensions;

public interface ILinqExtensions<TEntity> where TEntity : class
{
    Expression<Func<TEntity, bool>> Build<TFilter>(TFilter filters);
}
