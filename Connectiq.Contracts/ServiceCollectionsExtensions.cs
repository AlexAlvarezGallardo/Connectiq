using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Microsoft.Extensions.Hosting;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddContractsValidators(this IServiceCollection services)
        => services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    public static IServiceCollection AddContractsAutoMapper(this IServiceCollection services)
        => services.AddAutoMapper(x => { x.DisableConstructorMapping(); }, Assembly.GetExecutingAssembly());
}