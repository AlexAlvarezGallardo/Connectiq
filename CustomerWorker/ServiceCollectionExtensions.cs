using FluentValidation;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services) 
        => services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
}

