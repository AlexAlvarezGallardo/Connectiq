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

public class CreatedCustomerEventTests
{
    readonly Mock<IMapper> _mapperMock = new();
    readonly Mock<IRepository<CustomerEntity>> _repositoryMock = new();
    readonly Mock<ILogger<CreateCustomerEvent>> _loggerMock = new();

    [Fact]
    public async Task Consume_ValidCustomerValidated_InsertsCustomerEntityAndLogs()
    {
        var messageId = Guid.NewGuid();

        var customerValidated = new CustomerValidated
        {
            Customer = new Customers.Customer { Details = new CustomerDetails { Name = "Test" } },
            CreatedAt = DateTimeOffset.UtcNow,
            IsValid = true
        };
        var customerCreate = new CreateCustomer
        {
            CustomerValidated = customerValidated,
            EventId = messageId.ToString(),
            IsActive = true
        };
        var customerEntity = new CustomerEntity
        {
            Name = "Test Customer",
            Email = "test@gmail.com",
            Phone = "1234567890",
            Address = "123 Test St",
            CreatedAt = DateTimeOffset.UtcNow,
            IsActive = true
        };

        var contextMock = new Mock<ConsumeContext<CustomerValidated>>();
        contextMock.SetupGet(x => x.Message).Returns(customerValidated);
        contextMock.SetupGet(x => x.MessageId).Returns(messageId);

        _mapperMock.Setup(m => m.Map<CreateCustomer>(customerValidated)).Returns(customerCreate);
        _mapperMock.Setup(m => m.Map<CustomerEntity>(It.IsAny<CreateCustomer>())).Returns(customerEntity);

        var handler = new CreateCustomerEvent(_loggerMock.Object, _mapperMock.Object, _repositoryMock.Object);

        await handler.Consume(contextMock.Object);

        _mapperMock.Verify(m => m.Map<CreateCustomer>(It.Is<CustomerValidated>(cv => cv == customerValidated)), Times.Once);
        _mapperMock.Verify(m => m.Map<CustomerEntity>(It.Is<CreateCustomer>(cc => cc == customerCreate)), Times.Once);
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