﻿using Connectiq.Contracts.Customer;
using Connectiq.Tests.Utilities;
using FluentAssertions;
using System.Text.Json;
using Xunit;

namespace Connectiq.Contracts.Tests.Utilities;

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
    public void LoadFromFile_ShouldDeserializeJson_Throw_JsonException()
    {
        var path = JsonDataLoader.GetDataPath("InvalidCustomerInput.json");

        var act = () => JsonDataLoader.LoadFromFile<CreateCustomerInput>(path);

        act.Should()
            .Throw<JsonException>();
    }

    [Fact]
    public void LoadFromFile_ShouldDeserializeJson_Throw_InvalidOperationException()
    {
        var path = JsonDataLoader.GetDataPath("NullLiteral.json");

        var act = () => JsonDataLoader.LoadFromFile<CreateCustomerInput>(path);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Deserialization returned null");
    }
}
