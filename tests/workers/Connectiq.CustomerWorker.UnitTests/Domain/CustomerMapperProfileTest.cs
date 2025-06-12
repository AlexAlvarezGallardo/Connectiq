using AutoMapper;
using Connectiq.Tests.Utilities;
using Connectiq.Tests.Utilities.Fixtures;
using CustomerWorker.Domain.Commands;
using FluentAssertions;
using Xunit;

namespace Connectiq.Contracts.Tests.Customer;

public class CustomerMapperProfileTest : IClassFixture<MapperFixture>
{
    readonly IMapper _mapper;

    public CustomerMapperProfileTest(MapperFixture fixture)
    {
        _mapper = fixture.Mapper;
    }

    [Fact]
    public void Map_CreateCustomerInput_To_CustomerCreated_ShouldMapCorrectly()
    {
        var inputPath = JsonDataLoader.GetDataPath("CreateCustomerInput.json");
        var input = JsonDataLoader.LoadFromFile<CreateCustomerInput>(inputPath);
        var result = _mapper.Map<CustomerCreated>(input);

        result.CustomerData.Should().NotBeNull();
        result.CustomerData.Name.Should().Be("Juan");
        result.CustomerData.Address.Should().Be("Calle 123");
        result.CustomerData.Phone.Should().Be("555-1234");
        result.CustomerData.Email.Should().Be("juan@email.com");
    }

    [Fact]
    public void Map_CustomerCreated_To_CustomerValidated_ShouldMapCorrectly()
    {
        var inputPath = JsonDataLoader.GetDataPath("CustomerCreated.json");
        var input = JsonDataLoader.LoadFromFile<CustomerCreated>(inputPath);
        var result = _mapper.Map<CustomerValidated>(input);

        result.CustomerCreated.Should().BeEquivalentTo(input);
        result.IsValid.Should().BeTrue();
        result.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Map_CustomerCreated_To_CustomerNotValidated_ShouldMapCorrectly()
    {
        var inputPath = JsonDataLoader.GetDataPath("CustomerCreated.json");
        var input = JsonDataLoader.LoadFromFile<CustomerCreated>(inputPath);
        var result = _mapper.Map<CustomerNotValidated>(input);

        result.CustomerCreated.Should().BeEquivalentTo(input);
        result.CreatedAt.Should().Be(default);
        result.IsValid.Should().Be(default);
        result.NotValidatedMessage.Should().BeNull();
    }

    [Fact]
    public void Map_CustomerValidated_To_CustomerEntity_ShouldMapCorrectly()
    {
        var inputPath = JsonDataLoader.GetDataPath("CustomerValidated.json");
        var input = JsonDataLoader.LoadFromFile<CustomerValidated>(inputPath);

        var result = _mapper.Map<CustomerEntity>(input);

        result.Name.Should().Be("John");
        result.Address.Should().Be("Elm Street");
        result.Phone.Should().Be("111-222");
        result.Email.Should().Be("john@email.com");
        result.IsActive.Should().BeTrue();
        result.CreatedAt.Should().Be(input.CreatedAt);
    }
}
