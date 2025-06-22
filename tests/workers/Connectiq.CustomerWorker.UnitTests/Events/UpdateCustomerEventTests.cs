using AutoMapper;
using Connectiq.ProjectDefaults;
using CustomerWorker.Domain;
using CustomerWorker.Domain.Commands.UpdateCustomerCommand;
using CustomerWorker.Events;
using FluentValidation;
using FluentValidation.Results;
using global::CustomerWorker.Domain.Commands;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Connectiq.CustomerWorker.UnitTests.Events;

public class UpdateCustomerEventTests
{
    readonly Mock<IRepository<CustomerEntity>> _repositoryMock = new();
    readonly Mock<IMapper> _mapperMock = new();
    readonly Mock<IValidator<UpdateCustomer>> _validatorMock = new();
    readonly Mock<ILogger<UpdateCustomerEvent>> _loggerMock = new();

    [Fact]
    public async Task Consume_ValidMessage_UpdatesCustomer()
    {
        var customerValidated = new CustomerValidated
        {
            Customer = new Customers.Customer
            {
                Details = new Customers.CustomerDetails 
                {
                    Name = "Test",
                    Address = "Addr",
                    Phone = "123",
                    Email = "test@email.com",
                },
                Id = Guid.NewGuid().ToString()
            },
            CreatedAt = DateTimeOffset.UtcNow,
            IsValid = true
        };

        var updateCustomer = new UpdateCustomer
        {
            CustomerValidated = customerValidated,
            EventId = Guid.NewGuid().ToString(),
            IsActive = true
        };

        var customerEntity = new CustomerEntity
        {
            Id = Guid.Parse(customerValidated.Customer.Id),
            Name = customerValidated.Customer.Details.Name,
            Address = customerValidated.Customer.Details.Address,
            Phone = customerValidated.Customer.Details.Phone,
            Email = customerValidated.Customer.Details.Email,
            CreatedAt = customerValidated.CreatedAt,
            IsActive = true
        };

        var context = new Mock<ConsumeContext<CustomerValidated>>();
        context.SetupGet(x => x.Message).Returns(customerValidated);
        context.SetupGet(x => x.MessageId).Returns(Guid.NewGuid());

        _mapperMock.Setup(m => m.Map<UpdateCustomer>(It.IsAny<CustomerValidated>())).Returns(updateCustomer);
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateCustomer>(), default)).ReturnsAsync(new ValidationResult());
        _mapperMock.Setup(m => m.Map<CustomerEntity>(It.IsAny<UpdateCustomer>())).Returns(customerEntity);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<CustomerEntity>())).ReturnsAsync(true);

        var handler = new UpdateCustomerEvent(_loggerMock.Object, _validatorMock.Object, _mapperMock.Object, _repositoryMock.Object);

        await handler.Consume(context.Object);

        _repositoryMock.Verify(r => r.UpdateAsync(It.Is<CustomerEntity>(c => c.Id == customerEntity.Id)), Times.Once);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Customer updated")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);

    }

    [Fact]
    public async Task Consume_InvalidMessage_ThrowsValidationException()
    {
        var customerValidated = new CustomerValidated
        {
            Customer = new Customers.Customer
            {
                Details = new Customers.CustomerDetails
                {
                    Name = "",
                    Address = "Addr",
                    Phone = "123",
                    Email = "test@email.com",
                },
                Id = Guid.NewGuid().ToString()
            },
            CreatedAt = DateTimeOffset.UtcNow,
            IsValid = true
        };

        var updateCustomer = new UpdateCustomer
        {
            CustomerValidated = customerValidated,
            EventId = Guid.NewGuid().ToString(),
            IsActive = true
        };

        var failures = new[] { new ValidationFailure("Name", "Required") };
        var context = new Mock<ConsumeContext<CustomerValidated>>();
        context.SetupGet(x => x.Message).Returns(customerValidated);
        context.SetupGet(x => x.MessageId).Returns(Guid.NewGuid());

        _mapperMock.Setup(m => m.Map<UpdateCustomer>(It.IsAny<CustomerValidated>())).Returns(updateCustomer);
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateCustomer>(), default)).ReturnsAsync(new ValidationResult(failures));

        var handler = new UpdateCustomerEvent(_loggerMock.Object, _validatorMock.Object, _mapperMock.Object, _repositoryMock.Object);

        await Assert.ThrowsAsync<ValidationException>(() => handler.Consume(context.Object));
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<CustomerEntity>()), Times.Never);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Validation failed for UpdateCustomer")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }
}