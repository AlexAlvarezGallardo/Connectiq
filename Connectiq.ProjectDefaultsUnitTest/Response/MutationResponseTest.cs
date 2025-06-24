using Connectiq.ProjectDefaults.Response.Mutation;
using Connectiq.ProjectDefaults.Response.Query;
using FluentAssertions;
using FluentValidation.Results;

namespace Connectiq.ProjectDefaultsUnitTest.Response;

public class MutationResponseTests
{
    [Fact]
    public void Should_ReturnOkSuccessTrue_AndMessage()
    {
        var response = QueryResponse.Ok();

        response.Success.Should().BeTrue();
        response.Message.Should().BeEmpty();
    }
}

public class MutationResponseGenericTests
{
    [Fact]
    public void Should_ReturnOkSuccessTrue_AndData_AndMessage()
    {
        var data = 123;

        var response = MutationResponse<int>.Ok(data, "Created!");

        response.Success.Should().BeTrue();
        response.Data.Should().Be(data);
        response.Message.Should().Be("Created!");
        response.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Should_ReturnErrorSuccessFalse_AndData_AndMessage_AndErrors()
    {
        var data = 456;
        var errors = new List<ValidationFailure>
        {
            new("Field", "Invalid value")
        };

        var response = MutationResponse<int>.Error(data, errors, "Error!");

        response.Success.Should().BeFalse();
        response.Data.Should().Be(data);
        response.Message.Should().Be("Error!");
        response.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public void Should_ReturnEmptyErrors_WhenNullPassed()
    {
        var response = MutationResponse<string>.Error("data", null, "Error!");

        response.Errors.Should().BeEmpty();
    }
}
