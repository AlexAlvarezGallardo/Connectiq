using AutoMapper;
using Connectiq.Tests.Utilities;
using Connectiq.Tests.Utilities.Fixtures;
using Customers.Queries;
using CustomerWorker.Domain;
using FluentAssertions;
using Xunit;

namespace Connectiq.CustomerWorker.UnitTests.Domain.Queries;

public class CustomerQueriesMapperProfileTest(MapperFixture fixture) : IClassFixture<MapperFixture>
{
    readonly IMapper _mapper = fixture.Mapper;
    readonly string _basePath = "Customers/Queries";

    [Fact]
    public void Map_CustomerEntity_To_CustomerDto_ShouldMapCorrectly()
    {
        var inputPath = JsonDataLoader.GetDataPath($"{_basePath}/CustomerEntity.json");
        var input = JsonDataLoader.LoadFromFile<CustomerEntity>(inputPath);
        var result = _mapper.Map<CustomerDto>(input);

        result.Should().NotBeNull();
        result.Customer.Id.Should().Be("34f1fa5d-e996-4918-b196-da0caec905b6");
        result.Customer.Details.Name.Should().Be("John Doe");
        result.Customer.Details.Address.Should().Be("123 Main St, Springfield, IL 62701");
        result.Customer.Details.Phone.Should().Be("123-456-7890");
        result.Customer.Details.Email.Should().Be("john@gmail.com");
        result.CreatedAt.Should().Be("2023-01-01T00:00:00");
        result.IsActive.Should().BeTrue();
    }

}