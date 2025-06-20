using AutoMapper;
using Connectiq.ProjectDefaults;
using Customers;
using CustomerWorker.Domain;
using CustomerWorker.Domain.Commands;
using CustomerWorker.Domain.Commands.CreateCustomerCommand;
using CustomerWorker.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Connectiq.CustomerWorker.UnitTests.Events;

public class CustomerCreatedEventTests
{
    readonly Mock<IMapper> _mapperMock = new();
    readonly Mock<IRepository<CustomerEntity>> _repositoryMock = new ();
    readonly Mock<ILogger<CustomerCreateEvent>> _loggerMock = new ();

    [Fact]
    public async Task Consume_ValidCustomerValidated_InsertsCustomerEntityAndLogs()
    {
        var customerValidated = new CustomerValidated
        {
            Customer = new Customers.Customer { Details = new CustomerDetails { Name = "Test" } },
            CreatedAt = DateTimeOffset.UtcNow,
            IsValid = true
        };
        var customerCreate = new CreateCustomer 
        { 
            CustomerValidated = customerValidated,
            EventId = "event-id", 
            IsActive = false 
        };
        var customerEntity = new CustomerEntity
        {
            Name = "Test Customer",
            Email = "test@gmail.com",
            Phone = "1234567890",
            Address = "123 Test St",
        };

        var contextMock = new Mock<ConsumeContext<CustomerValidated>>();
        contextMock.SetupGet(x => x.Message).Returns(customerValidated);
        contextMock.SetupGet(x => x.MessageId).Returns(Guid.NewGuid());

        _mapperMock.Setup(m => m.Map<CreateCustomer>(customerValidated)).Returns(customerCreate);
        _mapperMock.Setup(m => m.Map<CustomerEntity>(It.IsAny<CreateCustomer>())).Returns(customerEntity);

        var handler = new CustomerCreateEvent(_loggerMock.Object, _mapperMock.Object, _repositoryMock.Object);

        // Act
        await handler.Consume(contextMock.Object);

        // Assert
        _repositoryMock.Verify(r => r.InsertAsync(It.Is<CustomerEntity>(e => e == customerEntity)), Times.Once);
        _loggerMock.Verify(
            l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Received CustomerCreatedEvent")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
        _loggerMock.Verify(
            l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Customer created with Id")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }
}