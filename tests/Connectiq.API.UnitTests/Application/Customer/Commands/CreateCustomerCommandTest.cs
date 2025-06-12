using AutoMapper;
using Connectiq.API.Application.Customer.Commands;
using CustomerWorker.Domain;
using Connectiq.Tests.Utilities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MassTransit;
using Moq;
using ValidationResult = FluentValidation.Results.ValidationResult;
using CustomerWorker.Domain.Commands;

namespace Connectiq.API.UnitTests.Application.Customer.Commands;


public class CreateCustomerCommandHandlerTests
{
    private readonly Mock<IPublishEndpoint> _publisherMock = new();
    private readonly Mock<IValidator<CustomerCreated>> _validatorMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    private readonly CreateCustomerCommandHandler _handler;

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
        var customerInputPath = JsonDataLoader.GetDataPath("CreateCustomerInput.json");
        var input = JsonDataLoader.LoadFromFile<CreateCustomerInput>(customerInputPath);

        var command = new CreateCustomerCommand(input);

        var customerCreated = new CustomerCreated
        {
            CustomerData = input.Customer,
            EventId = "event-1",
            IsActive = true
        };

        _mapperMock.Setup(m => m.Map<CustomerCreated>(input)).Returns(customerCreated);
        _validatorMock.Setup(v => v.ValidateAsync(customerCreated, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _publisherMock.Setup(p => p.Publish(customerCreated, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        _publisherMock.Verify(p => p.Publish(customerCreated, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_AndNotPublish_When_Validation_Fails()
    {
        var customerInputPath = JsonDataLoader.GetDataPath("CreateCustomerInput.json");
        var input = JsonDataLoader.LoadFromFile<CreateCustomerInput>(customerInputPath);

        var command = new CreateCustomerCommand(input);

        var customerCreated = new CustomerCreated
        {
            CustomerData = input.Customer,
            EventId = "event-1",
            IsActive = true
        };

        var failures = new[] { new ValidationFailure("Field", "Error") };
        _mapperMock.Setup(m => m.Map<CustomerCreated>(input)).Returns(customerCreated);
        _validatorMock.Setup(v => v.ValidateAsync(customerCreated, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeFalse();
        _publisherMock.Verify(p => p.Publish(It.IsAny<CustomerCreated>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}