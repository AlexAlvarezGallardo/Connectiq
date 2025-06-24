using AutoMapper;
using Connectiq.Tests.Utilities;
using Connectiq.Tests.Utilities.Fixtures;
using Customers;
using Customers.Commands;
using CustomerWorker.Domain;
using CustomerWorker.Domain.Commands;
using CustomerWorker.Domain.Commands.CreateCustomerCommand;
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

    [Fact]
    public void Map_CreateCustomerInput_To_CustomerValidated_ShouldMapCorrectly()
    {
        var inputPath = JsonDataLoader.GetDataPath($"{_basePath}/Input/CreateCustomerInput.json");
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
    public void Map_CustomerValidated_To_CustomerBase_ShouldMapCorrectly()
    {
        var inputPath = JsonDataLoader.GetDataPath($"{_basePath}/CustomerValidated.json");
        var input = JsonDataLoader.LoadFromFile<CustomerValidated>(inputPath);
        var result = _mapper.Map<CustomerBase>(input);

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
        var inputPath = JsonDataLoader.GetDataPath($"{_basePath}/CustomerCreate.json");
        var input = JsonDataLoader.LoadFromFile<CreateCustomer>(inputPath);
        var result = _mapper.Map<CustomerEntity>(input);

        result.Name.Should().Be("John");
        result.Address.Should().Be("Elm Street");
        result.Phone.Should().Be("111-222");
        result.Email.Should().Be("john@email.com");
    }
}
