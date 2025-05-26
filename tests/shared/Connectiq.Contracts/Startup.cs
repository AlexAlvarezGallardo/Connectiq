using Connectiq.Contracts.Customer;
using Microsoft.Extensions.DependencyInjection;

namespace Connectiq.Contracts.Tests;

public class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<CustomerMapperProfile>());
    }
}
