using LinqKit;
using System.Linq.Expressions;

namespace Connectiq.ProjectDefaults.LinqExtensions;

public class LinqExtensions<TEntity> : ILinqExtensions<TEntity> where TEntity : class
{
    public Expression<Func<TEntity, bool>> Build<TFilter>(TFilter filters)
    {
        if(filters is null)
            return PredicateBuilder.New<TEntity>(true);

        var predicate = PredicateBuilder.New<TEntity>(true);
        var properties = typeof(TFilter).GetProperties();

        foreach (var prop in properties)
        {
            var value = prop.GetValue(filters);
            if (value is string str && !string.IsNullOrWhiteSpace(str))
            {
                var param = Expression.Parameter(typeof(TEntity), typeof(TEntity).Name);
                var customerProp = Expression.Property(param, prop.Name);
                var constant = Expression.Constant(str);
                var contains = Expression.Call(
                    customerProp,
                    typeof(string).GetMethod("Contains", [typeof(string)])!,
                    constant
                );

                var lambda = Expression.Lambda<Func<TEntity, bool>>(contains, param);
                predicate = predicate.And(lambda);
            }
        }

        return predicate;
    }
}
