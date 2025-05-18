using AutoMapper;
using Connectiq.Contracts.Customer;
using Connectiq.Tests.Utilities;
using FluentAssertions;
using Xunit;

namespace Connectiq.Contracts.Tests;

public class CustomerMapperProfileTest(IMapper _mapper)
{
    [Fact]
    public void Map_CreateCustomerInput_To_CustomerCreated_ShouldMapCorrectly()
    {
        var input = JsonDataLoader.LoadFromFile<CreateCustomerInput>("TestData/Customers/CreateCustomerInput.json");
        var result = _mapper.Map<CustomerCreated>(input);

        result.CustomerData.Should().NotBeNull();
        result.CustomerData.Name.Should().Be("Ana");
        result.CustomerData.Address.Should().Be("Main St");
        result.CustomerData.Phone.Should().Be("123-456");
        result.CustomerData.Email.Should().Be("ana@mail.com");
    }

    [Fact]
    public void Map_CustomerCreated_To_CustomerValidated_ShouldMapCorrectly()
    {
        var input = JsonDataLoader.LoadFromFile<CustomerCreated>("TestData/Customers/CustomerCreated.json");
        var result = _mapper.Map<CustomerValidated>(input);

        result.CustomerCreated.Should().BeEquivalentTo(input);
        result.IsValid.Should().BeTrue();
        result.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Map_CustomerCreated_To_CustomerNotValidated_ShouldMapCorrectly()
    {
        var input = JsonDataLoader.LoadFromFile<CustomerCreated>("TestData/Customers/CustomerCreated.json");
        var result = _mapper.Map<CustomerNotValidated>(input);

        result.CustomerCreated.Should().BeEquivalentTo(input);
        result.CreatedAt.Should().Be(default);
        result.IsValid.Should().Be(default);
        result.NotValidatedMessage.Should().BeNull();
    }

    [Fact]
    public void Map_CustomerValidated_To_CustomerEntity_ShouldMapCorrectly()
    {
        var input = JsonDataLoader.LoadFromFile<CustomerValidated>("TestData/Customers/CustomerValidatedToEntity.json");
        var result = _mapper.Map<CustomerEntity>(input);

        result.Name.Should().Be("John");
        result.Address.Should().Be("Elm Street");
        result.Phone.Should().Be("111-222");
        result.Email.Should().Be("john@email.com");
        result.IsActive.Should().BeTrue();
        result.CreatedAt.Should().Be(input.CreatedAt);
    }
}
