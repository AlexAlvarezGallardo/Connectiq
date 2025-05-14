using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Microsoft.Extensions.Hosting;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    => services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
}