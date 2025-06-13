using AutoMapper;
using CustomerWorker.Domain.Commands;

namespace Connectiq.Tests.Utilities.Fixtures;

public class MapperFixture
{
    public IMapper Mapper { get; }

    public MapperFixture()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapperProfile>();
        });

        Mapper = config.CreateMapper();
    }
}