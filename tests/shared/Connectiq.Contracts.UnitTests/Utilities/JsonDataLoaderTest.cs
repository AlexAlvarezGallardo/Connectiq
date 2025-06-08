using Connectiq.Contracts.Customer;
using Connectiq.Tests.Utilities;
using FluentAssertions;
using System.Text.Json;
using Xunit;

namespace Connectiq.Contracts.UnitTests.Utilities;

public class JsonDataLoaderTest
{
    readonly string _jsonDataEntity = typeof(GrpcCustomers.Customer).Name;

    [Fact]
    public void LoadFromFile_ShouldDeserializeJson()
    {
        var path = JsonDataLoader.GetDataPath(_jsonDataEntity, "CreateCustomerInput.json");
        var result = JsonDataLoader.LoadFromFile<CreateCustomerInput>(path);

        result.Should().NotBeNull();
    }

    [Fact]
    public void LoadFromFile_ShouldDeserializeJson_Throw_FileNotFoundException()
    {   
        var path = JsonDataLoader.GetDataPath(_jsonDataEntity, "NotExist.json");

        var act = () => JsonDataLoader.LoadFromFile<CreateCustomerInput>(path);

        act.Should()
            .Throw<FileNotFoundException>()
            .WithMessage($"Test data file not found: {Path.Combine(Directory.GetCurrentDirectory(), path)}");
    }

    [Fact]
    public void LoadFromFile_ShouldDeserializeJson_Throw_JsonException()
    {
        var path = JsonDataLoader.GetDataPath(_jsonDataEntity, "InvalidCustomerInput.json");

        var act = () => JsonDataLoader.LoadFromFile<CreateCustomerInput>(path);

        act.Should()
            .Throw<JsonException>();
    }

    [Fact]
    public void LoadFromFile_ShouldDeserializeJson_Throw_InvalidOperationException()
    {
        var path = JsonDataLoader.GetDataPath(_jsonDataEntity, "NullLiteral.json");

        var act = () => JsonDataLoader.LoadFromFile<CreateCustomerInput>(path);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Deserialization returned null");
    }
}
