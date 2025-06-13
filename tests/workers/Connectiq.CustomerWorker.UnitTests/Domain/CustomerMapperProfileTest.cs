using AutoMapper;
using Connectiq.Tests.Utilities;
using Connectiq.Tests.Utilities.Fixtures;
using Customers;
using CustomerWorker.Domain;
using CustomerWorker.Domain.Commands;
using FluentAssertions;
using Xunit;

namespace Connectiq.CustomerWorker.UnitTests.Domain;

public class CustomerMapperProfileTest(MapperFixture fixture) : IClassFixture<MapperFixture>
{
    readonly IMapper _mapper =  fixture.Mapper;

    [Fact]
    public void Map_CreateCustomerInput_To_CustomerValidated_ShouldMapCorrectly()
    {
        var inputPath = JsonDataLoader.GetDataPath("CreateCustomerInput.json");
        var input = JsonDataLoader.LoadFromFile<CreateCustomerInput>(inputPath);
        var result = _mapper.Map<CustomerValidated>(input);

        result.Customer.Should().NotBeNull();
        result.Customer.Details.Name.Should().Be("John");
        result.Customer.Details.Address.Should().Be("Elm Street");
        result.Customer.Details.Phone.Should().Be("111-222");
        result.Customer.Details.Email.Should().Be("john@email.com");
        result.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Map_CreateCustomerInput_To_CustomerNotValidated_ShouldMapCorrectly()
    {
        var inputPath = JsonDataLoader.GetDataPath("CreateCustomerInput.json");
        var input = JsonDataLoader.LoadFromFile<CreateCustomerInput>(inputPath);
        var result = _mapper.Map<CustomerNotValidated>(input);

        result.Customer.Should().NotBeNull();
        result.Customer.Details.Name.Should().Be("John");
        result.Customer.Details.Address.Should().Be("Elm Street");
        result.Customer.Details.Phone.Should().Be("111-222");
        result.Customer.Details.Email.Should().Be("john@email.com");
        result.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Map_CustomerValidated_To_CustomerCreate_ShouldMapCorrectly()
    {
        var inputPath = JsonDataLoader.GetDataPath("CustomerValidated.json");
        var input = JsonDataLoader.LoadFromFile<CustomerValidated>(inputPath);
        var result = _mapper.Map<CustomerCreate>(input);

        result.CustomerValidated.Customer.Details.Name.Should().Be("John");
        result.CustomerValidated.Customer.Details.Address.Should().Be("Elm Street");
        result.CustomerValidated.Customer.Details.Phone.Should().Be("111-222");
        result.CustomerValidated.Customer.Details.Email.Should().Be("john@email.com");
        result.CustomerValidated.CreatedAt.Should().Be(DateTimeOffset.Parse("2025-05-15T10:00:00+00:00"));
        result.CustomerValidated.IsValid.Should().BeTrue();
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Map_CustomerCreate_To_CustomerEntity_ShouldMapCorrectly()
    {
        var inputPath = JsonDataLoader.GetDataPath("CustomerCreate.json");
        var input = JsonDataLoader.LoadFromFile<CustomerCreate>(inputPath);
        var result = _mapper.Map<CustomerEntity>(input);

        result.Name.Should().Be("John");
        result.Address.Should().Be("Elm Street");
        result.Phone.Should().Be("111-222");
        result.Email.Should().Be("john@email.com");
    }
}
