using Connectiq.ProjectDefaults;
using Connectiq.ProjectDefaults.LinqExtensions;
using Connectiq.ProjectDefaults.Repository;
using Connectiq.ProjectDefaults.Response.Mutation;
using Connectiq.ProjectDefaults.Response.Mutation.Factory;
using Connectiq.ProjectDefaults.Response.Query;
using Connectiq.ProjectDefaults.Response.Query.Factory;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConnectiqProjectDefaults<TEntity>(this IServiceCollection services)
        where TEntity : class, ISoftDelete
        => services
            .AddScoped<ILinqExtensions<TEntity>, LinqExtensions<TEntity>>()
            .AddScoped<IRepository<TEntity>, DbRepository<TEntity>>();

    public static IServiceCollection AddMutationResponseFactory<TValidated>(this IServiceCollection services)
        where TValidated : class
        => services
            .AddScoped<IMutationResultFactory, MutationResultFactory>()
            .AddScoped<IMutationResponse<TValidated>, MutationResponse<TValidated>>();

    public static IServiceCollection AddQueryResponseFactory<TInput>(this IServiceCollection services)
        => services
            .AddScoped<IQueryResultFactory, QueryResultFactory>()
            .AddScoped<IQueryResponse<TInput>, QueryResponse<TInput>>();
}
