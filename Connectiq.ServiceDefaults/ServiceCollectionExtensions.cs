using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Microsoft.Extensions.Hosting;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidators<T>(this IServiceCollection services)
    => services
        .AddValidatorsFromAssembly(Assembly.GetCallingAssembly())
        .AddValidatorsFromAssembly(typeof(T).Assembly);

    public static IServiceCollection AddAutoMapper<T>(this IServiceCollection services)
        => services.AddAutoMapper(x => { x.DisableConstructorMapping(); }, typeof(T).Assembly);
}

