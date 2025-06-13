using Customers;
using FluentAssertions;
using System.Text.Json;
using Xunit;

namespace Connectiq.Tests.Utilities;

public class JsonDataLoaderTest
{
    [Fact]
    public void LoadFromFile_ShouldDeserializeJson()
    {
        var path = JsonDataLoader.GetDataPath("CreateCustomerInput.json");
        var result = JsonDataLoader.LoadFromFile<CreateCustomerInput>(path);

        result.Should().NotBeNull();
    }

    [Fact]
    public void LoadFromFile_ShouldDeserializeJson_Throw_FileNotFoundException()
    {   
        var path = JsonDataLoader.GetDataPath("NotExist.json");

        var act = () => JsonDataLoader.LoadFromFile<CreateCustomerInput>(path);

        act.Should()
            .Throw<FileNotFoundException>()
            .WithMessage($"Test data file not found: {Path.Combine(Directory.GetCurrentDirectory(), path)}");
    }

    [Fact]
    public void LoadFromFile_ShouldDeserializeJson_Throw_InvalidOperationException()
    {
        var path = JsonDataLoader.GetDataPath("..//NullLiteral.json");

        var act = () => JsonDataLoader.LoadFromFile<CreateCustomerInput>(path);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Deserialization returned null");
    }
}
