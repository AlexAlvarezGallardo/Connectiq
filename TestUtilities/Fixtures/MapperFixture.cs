using AutoMapper;
using Connectiq.Contracts.Customer;

namespace Connectiq.Tests.Utilities.Fixtures;

public class MapperFixture
{
    public IMapper Mapper { get; }

    public MapperFixture()
    {
        var config = new MapperConfiguration(cfg => 
        { 
            cfg.AddProfile<CustomerMapperProfile>(); 
        });
        Mapper = config.CreateMapper();
    }
}
