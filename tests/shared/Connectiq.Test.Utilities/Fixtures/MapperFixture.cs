using AutoMapper;
using CustomerWorker.Domain.Commands;
using CustomerWorker.Domain.Commands.CreateCustomerCommand;
using CustomerWorker.Domain.Queries;

namespace Connectiq.Tests.Utilities.Fixtures;

public class MapperFixture
{
    public IMapper Mapper { get; }

    public MapperFixture()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GlobalCustomerMapperProfile>();
            cfg.AddProfile<CreateCustomerMapperProfile>();
            cfg.AddProfile<CustomerQueryMapperProfile>();
        });

        Mapper = config.CreateMapper();
    }
}