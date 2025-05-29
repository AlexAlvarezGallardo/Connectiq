using AutoMapper;
using Connectiq.Contracts.Customer;
using Connectiq.Tests.Utilities;
using CustomerWorker.Events;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Connectiq.CustomerWorker.Tests.Events;

public class CustomerCreatedEventTests
{
    readonly Mock<ILogger<CustomerCreatedEvent>> _loggerMock = new();
    readonly Mock<IMapper> _mapperMock = new();
    readonly Mock<IValidator<CustomerValidated>> _validatorMock = new();
    readonly Mock<IPublishEndpoint> _busMock = new();

    readonly CustomerCreatedEvent _consumer;

    public CustomerCreatedEventTests()
    {
        _consumer = new CustomerCreatedEvent(
            _loggerMock.Object,
            _mapperMock.Object,
            _validatorMock.Object,
            _busMock.Object);
    }

    [Fact]
    public async Task Consume_ShouldPublishValidatedCustomer_WhenValidationSucceeds()
    {
        // Arrange
        var customerCreatedPath = JsonDataLoader.GetDataPath("CustomerCreated.json");
        var customerValidatedPath = JsonDataLoader.GetDataPath("CustomerValidated.json");

        var customerCreated = JsonDataLoader.LoadFromFile<CustomerCreated>(customerCreatedPath);
        var expectedValidated = JsonDataLoader.LoadFromFile<CustomerValidated>(customerValidatedPath);

        var contextMock = new Mock<ConsumeContext<CustomerCreated>>();
        contextMock.Setup(c => c.Message).Returns(customerCreated);
        contextMock.Setup(c => c.MessageId).Returns(Guid.NewGuid());

        CustomerValidated? publishedMessage = null;

        _mapperMock.Setup(m => m.Map<CustomerValidated>(It.IsAny<CustomerCreated>()))
            .Returns((CustomerCreated input) =>
            {
                return expectedValidated with
                {
                    CustomerCreated = input with
                    {
                        EventId = expectedValidated.CustomerCreated.EventId,
                        IsActive = true
                    }
                };
            });

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CustomerValidated>(), default))
            .ReturnsAsync(new ValidationResult());

        _busMock.Setup(b => b.Publish(It.IsAny<CustomerValidated>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((msg, _) =>
            {
                publishedMessage = msg as CustomerValidated;
            });

        await _consumer.Consume(contextMock.Object);

        publishedMessage.Should().NotBeNull();
        publishedMessage.Should().BeEquivalentTo(expectedValidated, options =>
            options.Excluding(x => x.CustomerCreated.EventId));
    }


    [Fact]
    public async Task Consume_ShouldPublishNotValidatedCustomer_WhenValidationFails()
    {
        var customerCreatedPath = JsonDataLoader.GetDataPath("CustomerCreated.json");
        var customerValidatedPath = JsonDataLoader.GetDataPath("CustomerValidated.json");
        var customerNotValidatedPath = JsonDataLoader.GetDataPath("CustomerNotValidated.json");

        var customerCreated = JsonDataLoader.LoadFromFile<CustomerCreated>(customerCreatedPath);
        var customerValidated = JsonDataLoader.LoadFromFile<CustomerValidated>(customerValidatedPath);
        var expectedNotValidated = JsonDataLoader.LoadFromFile<CustomerNotValidated>(customerNotValidatedPath);

        var contextMock = new Mock<ConsumeContext<CustomerCreated>>();
        contextMock.Setup(c => c.Message).Returns(customerCreated);
        contextMock.Setup(c => c.MessageId).Returns(Guid.NewGuid());

        _mapperMock.Setup(m => m.Map<CustomerValidated>(customerCreated)).Returns(customerValidated);
        _mapperMock.Setup(m => m.Map<CustomerNotValidated>(customerCreated)).Returns(expectedNotValidated);

        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CustomerValidated>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
            {
            new ValidationFailure("Name", "Name is required")
            }));

        CustomerNotValidated? actualPublished = null;
        _busMock.Setup(b => b.Publish(It.IsAny<CustomerNotValidated>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((msg, _) => actualPublished = msg as CustomerNotValidated);

        await _consumer.Consume(contextMock.Object);

        actualPublished.Should().NotBeNull();
        actualPublished.Should().BeEquivalentTo(expectedNotValidated, options =>
            options.Excluding(x => x.NotValidatedMessage)); 

        _busMock.Verify(b => b.Publish(It.IsAny<CustomerNotValidated>(), contextMock.Object.CancellationToken), Times.Once);
    }

}
