using AutoMapper;
using Connectiq.Contracts.Customer;
using Connectiq.Contracts.User;

namespace Connectiq.Tests.Utilities.Fixtures;

public class MapperFixture
{
    public IMapper Mapper { get; }

    public MapperFixture()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CustomerMapperProfile>();
            cfg.AddProfile<UserMapperProfile>();
        });

        Mapper = config.CreateMapper();
    }
}