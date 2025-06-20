
using Connectiq.API.Application.Behaviors;
using Connectiq.ProjectDefaults.Response.Factory;
using Connectiq.ProjectDefaults.Response.Factory.Mutation;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using System.Net;

namespace Connectiq.API.UnitTests.Application.Behavior;

public class ValidationBehaviorTests
{
    public class TestRequest { }
    public class TestResponse : MutationResponse<TestRequest> { }

    [Fact]
    public async Task Handle_NoValidators_ShouldCallsNext()
    {
        var validators = Enumerable.Empty<IValidator<TestRequest>>();
        var responseFactory = new Mock<IMutationResultFactory>();
        var expectedResponse = new TestResponse();
        var nextCalled = false;

        Task<TestResponse> next(CancellationToken cancellationToken = default)
        {
            nextCalled = true;
            return Task.FromResult(expectedResponse);
        }

        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators, responseFactory.Object);

        var result = await behavior.Handle(new TestRequest(), next, CancellationToken.None);

        nextCalled.Should().BeTrue();
        result.Should().BeSameAs(expectedResponse);
    }

    [Fact]
    public async Task Handle_ValidationFails_ShouldReturnErrorResponse()
    {
        var failure = new ValidationFailure("Prop", "Error");
        var validationResult = new ValidationResult([failure]);

        var validator = new Mock<IValidator<TestRequest>>();
        validator.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(validationResult);

        var errorResponse = new TestResponse
        {
            Success = false,
            Errors = [failure],
            StatusCode = HttpStatusCode.BadRequest
        };

        var responseFactory = new Mock<IMutationResultFactory>();
        responseFactory.Setup(f =>
                f.Error(It.IsAny<TestRequest>(), It.IsAny<List<ValidationFailure>>(), It.IsAny<string>(), It.IsAny<HttpStatusCode>()))
                       .Returns(errorResponse);

        static Task<TestResponse> next(CancellationToken _ = default) => throw new Exception("Should not be called");

        var behavior = new ValidationBehavior<TestRequest, TestResponse>(
            [validator.Object], responseFactory.Object);

        var result = await behavior.Handle(new TestRequest(), next, CancellationToken.None);

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle().Which.Should().BeEquivalentTo(failure);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Handle_ValidationPasses_ShouldCallsNext()
    {
        var validationResult = new ValidationResult(); 

        var validator = new Mock<IValidator<TestRequest>>();
        validator.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(validationResult);

        var expectedResponse = new TestResponse { Success = true };
        var responseFactory = new Mock<IMutationResultFactory>();

        Task<TestResponse> next(CancellationToken cancellationToken = default) => Task.FromResult(expectedResponse);

        var behavior = new ValidationBehavior<TestRequest, TestResponse>(
            [validator.Object], responseFactory.Object);

        var result = await behavior.Handle(new TestRequest(), next, CancellationToken.None);

        result.Should().BeSameAs(expectedResponse);
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_MultipleValidators_ShouldCombinesFailures()
    {
        var failure1 = new ValidationFailure("Field1", "Error1");
        var failure2 = new ValidationFailure("Field2", "Error2");

        var validator1 = new Mock<IValidator<TestRequest>>();
        validator1.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ValidationResult([failure1]));

        var validator2 = new Mock<IValidator<TestRequest>>();
        validator2.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ValidationResult([failure2]));

        var responseFactory = new Mock<IMutationResultFactory>();
        var errorResponse = new TestResponse
        {
            Success = false,
            Errors = [failure1, failure2],
            StatusCode = HttpStatusCode.BadRequest
        };

        responseFactory.Setup(f => f.Error(
            It.IsAny<TestRequest>(),
            It.Is<List<ValidationFailure>>(f => f.Count == 2),
            It.IsAny<string>(),
            It.IsAny<HttpStatusCode>()))
            .Returns(errorResponse);

        var behavior = new ValidationBehavior<TestRequest, TestResponse>(
            [validator1.Object, validator2.Object], responseFactory.Object);

        var result = await behavior.Handle(new TestRequest(), _ => Task.FromResult(new TestResponse()), CancellationToken.None); 

        result.Success.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors.Should().ContainEquivalentOf(failure1);
        result.Errors.Should().ContainEquivalentOf(failure2);
    }
}
