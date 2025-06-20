using AutoMapper;
using Connectiq.Tests.Utilities;
using Connectiq.Tests.Utilities.Fixtures;
using Customers;
using FluentAssertions;
using Xunit;

namespace Connectiq.CustomerWorker.UnitTests.Domain.Commands;

public class GlobalCustomerMapperProfileTest(MapperFixture fixture) : IClassFixture<MapperFixture>
{
    private readonly IMapper _mapper = fixture.Mapper;
    readonly string _basePath = "Customers/Commands";

    [Fact]
    public void Map_CustomerDetails_To_Customer_ShouldMapCorrectly()
    {
        var inputPath = JsonDataLoader.GetDataPath($"{_basePath}/CustomerDetails.json");
        var input = JsonDataLoader.LoadFromFile<CustomerDetails>(inputPath);
        var result = _mapper.Map<Customers.Customer>(input);

        result.Should().NotBeNull();
        result.Details.Name.Should().Be("John");
        result.Details.Address.Should().Be("Elm Street");
        result.Details.Phone.Should().Be("111-222");
        result.Details.Email.Should().Be("john@email.com");
    }
}
