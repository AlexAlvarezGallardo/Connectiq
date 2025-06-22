using Connectiq.ProjectDefaults;
using Connectiq.ProjectDefaults.LinqExtensions;
using Connectiq.ProjectDefaults.Repository;
using Connectiq.ProjectDefaults.Response.Factory;
using Connectiq.ProjectDefaults.Response.Factory.Mutation;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConnectiqProjectDefaults<TEntity>(this IServiceCollection services)
        where TEntity : class, ISoftDelete
        => services
            .AddScoped<ILinqExtensions<TEntity>, LinqExtensions<TEntity>>()
            .AddScoped<IRepository<TEntity>, DbRepository<TEntity>>();

    public static IServiceCollection AddResponseFactory<TValidated>(this IServiceCollection services)
        where TValidated : class
        => services
            .AddScoped<IMutationResultFactory, MutationResultFactory>()
            .AddScoped<IMutationResponse<TValidated>, MutationResponse<TValidated>>();
}
