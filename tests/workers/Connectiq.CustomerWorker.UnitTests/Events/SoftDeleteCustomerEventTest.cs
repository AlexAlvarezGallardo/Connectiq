﻿using AutoMapper;
using Connectiq.ProjectDefaults;
using Customers;
using CustomerWorker.Domain;
using CustomerWorker.Domain.Commands;
using CustomerWorker.Domain.Commands.SoftDeleteCustomerCommand;
using CustomerWorker.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Connectiq.CustomerWorker.UnitTests.Events;

public class SoftDeleteCustomerEventTest
{
    readonly Mock<IMapper> _mapperMock = new();
    readonly Mock<IRepository<CustomerEntity>> _repositoryMock = new();
    readonly Mock<ILogger<SoftDeleteCustomerEvent>> _loggerMock = new();

    [Fact]
    public async Task Consume_ValidCustomerValidated_SoftDeleteCustomerEntityAndLogs()
    {
        var customerValidated = new CustomerValidated
        {
            Customer = new Customers.Customer { Details = new CustomerDetails { Name = "Test" } },
            CreatedAt = DateTimeOffset.UtcNow,
            IsValid = true
        };
        var customerDelete = new SoftDeleteCustomer
        {
            Id = Guid.NewGuid(),
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

        _mapperMock.Setup(m => m.Map<SoftDeleteCustomer>(customerValidated)).Returns(customerDelete);
        _mapperMock.Setup(m => m.Map<CustomerEntity>(It.IsAny<SoftDeleteCustomer>())).Returns(customerEntity);

        var handler = new SoftDeleteCustomerEvent(_loggerMock.Object, _mapperMock.Object, _repositoryMock.Object);

        await handler.Consume(contextMock.Object);

        _repositoryMock.Verify(r => r.SoftDeleteAsync(It.Is<CustomerEntity>(e => e == customerEntity)), Times.Once);
        _mapperMock.Verify(m => m.Map<SoftDeleteCustomer>(It.Is<CustomerValidated>(cv => cv == customerValidated)), Times.Once);
        _mapperMock.Verify(m => m.Map<CustomerEntity>(It.Is<SoftDeleteCustomer>(sd => sd == customerDelete)), Times.Once);
        _loggerMock.Verify(
            l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Received SoftDeleteCustomerEvent")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
        _loggerMock.Verify(
            l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Customer soft deleted with Id")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }
}