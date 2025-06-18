namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    => services
            .AddDbContext<CustomerDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("customers"));
            })
        .AddScoped<DbContext>(provider => provider.GetRequiredService<CustomerDbContext>());
}
