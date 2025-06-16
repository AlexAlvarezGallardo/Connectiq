using AutoMapper;
using Connectiq.API.Application.Customer.Commands;
using Connectiq.Tests.Utilities;
using Customers.Commands;
using CustomerWorker.Domain.Commands;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MassTransit;
using Moq;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Connectiq.API.UnitTests.Application.Customer.Commands;

public class CreateCustomerCommandHandlerTests
{
    readonly Mock<IPublishEndpoint> _publisherMock = new();
    readonly Mock<IValidator<CustomerValidated>> _validatorMock = new();
    readonly Mock<IMapper> _mapperMock = new();
    readonly CreateCustomerCommandHandler _handler;

    readonly string _basePath = "Customers/Commands";

    public CreateCustomerCommandHandlerTests()
    {
        _handler = new CreateCustomerCommandHandler(
            _publisherMock.Object,
            _validatorMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_And_Publish_When_Validation_Succeeds()
    {
        var customerInputPath = JsonDataLoader.GetDataPath($"{_basePath}/CreateCustomerInput.json");
        var input = JsonDataLoader.LoadFromFile<CreateCustomerInput>(customerInputPath);

        var command = new CreateCustomerCommand(input);

        var customerValidated = new CustomerValidated
        {
            Customer = new Customers.Customer() { Details = input.Details },
            CreatedAt = DateTimeOffset.UtcNow,
            IsValid = true
        };

        _mapperMock.Setup(m => m.Map<CustomerValidated>(input)).Returns(customerValidated);
        _validatorMock.Setup(v => v.ValidateAsync(customerValidated, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _publisherMock.Setup(p => p.Publish(customerValidated, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        _publisherMock.Verify(p => p.Publish(customerValidated, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_AndNotPublish_When_Validation_Fails()
    {
        var customerInputPath = JsonDataLoader.GetDataPath($"{_basePath}/CreateCustomerInput.json");
        var input = JsonDataLoader.LoadFromFile<CreateCustomerInput>(customerInputPath);

        var command = new CreateCustomerCommand(input);

        var customerValidated = new CustomerValidated
        {
            Customer = new Customers.Customer() { Details = input.Details },
            CreatedAt = DateTimeOffset.UtcNow,
            IsValid = true
        };

        var failures = new[] { new ValidationFailure("Field", "Error") };
        _mapperMock.Setup(m => m.Map<CustomerValidated>(input)).Returns(customerValidated);
        _validatorMock.Setup(v => v.ValidateAsync(customerValidated, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeFalse();
        _publisherMock.Verify(p => p.Publish(It.IsAny<CustomerCreate>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}