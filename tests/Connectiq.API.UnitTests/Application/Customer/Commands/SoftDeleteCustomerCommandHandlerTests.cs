﻿using AutoMapper;
using Connectiq.API.Application.Customer.Commands;
using Connectiq.ProjectDefaults.EventBus;
using Connectiq.ProjectDefaults.Response.Mutation;
using Connectiq.ProjectDefaults.Response.Mutation.Factory;
using Connectiq.Tests.Utilities;
using Customers.Commands;
using CustomerWorker.Domain.Commands;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MassTransit;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Connectiq.API.UnitTests.Application.Customer.Commands;

public class SoftDeleteCustomerCommandHandlerTests
{
    readonly Mock<IBus> _busMock = new();
    readonly Mock<IValidator<SoftDeleteCustomerInput>> _validatorMock = new();
    readonly Mock<IMapper> _mapperMock = new();
    readonly Mock<IMutationResultFactory> _mutationResult = new();
    readonly Mock<IOptions<EventBusOptions>> _optionsMock = new();
    readonly SoftDeleteCustomerCommandHandler _handler;

    readonly string _basePath = "Customers/Commands/Input";

    public SoftDeleteCustomerCommandHandlerTests()
    {
        _handler = new SoftDeleteCustomerCommandHandler(
            _busMock.Object,
            _validatorMock.Object,
            _mutationResult.Object,
            _mapperMock.Object,
            _optionsMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_And_Publish_When_Validation_Succeeds()
    {
        var softDeleteCustomerInputPath = JsonDataLoader.GetDataPath($"{_basePath}/SoftDeleteCustomerInput.json");
        var input = JsonDataLoader.LoadFromFile<SoftDeleteCustomerInput>(softDeleteCustomerInputPath);

        var command = new SoftDeleteCustomerCommand(input);

        var customerValidated = new CustomerValidated
        {
            Customer = new Customers.Customer() { Id = input.Id },
            CreatedAt = DateTimeOffset.UtcNow,
            IsValid = true
        };

        _mapperMock.Setup(m => m.Map<CustomerValidated>(input)).Returns(customerValidated);
        _validatorMock.Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _mutationResult.Setup(f => f.Ok(It.IsAny<CustomerValidated>(), It.IsAny<string>()))
            .Returns(new MutationResponse<CustomerValidated> { Success = true, Data = customerValidated, Message = string.Empty });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(customerValidated);
        result.Errors.Should().BeNullOrEmpty();

        _validatorMock.Verify(v => v.ValidateAsync(It.IsAny<SoftDeleteCustomerInput>(), It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<CustomerValidated>(It.IsAny<SoftDeleteCustomerInput>()), Times.Once);
        _mutationResult.Verify(m => m.Error(It.IsAny<CustomerValidated>(), It.IsAny<List<ValidationFailure>>(), It.IsAny<string>(), HttpStatusCode.BadRequest), Times.Never);
        _mutationResult.Verify(m => m.Ok(It.IsAny<CustomerValidated>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnErrorResponse_AndNotPublish_When_Validation_Fails()
    {
        var softDeleteCustomerInputPath = JsonDataLoader.GetDataPath($"{_basePath}/SoftDeleteCustomerInput.json");
        var input = JsonDataLoader.LoadFromFile<SoftDeleteCustomerInput>(softDeleteCustomerInputPath);

        var command = new SoftDeleteCustomerCommand(input);

        var customerValidated = new CustomerValidated
        {
            Customer = new Customers.Customer() { Id = input.Id },
            CreatedAt = DateTimeOffset.UtcNow,
            IsValid = false
        };

        var failures = new List<ValidationFailure> { new("Field", "Error") };
        var validationResult = new ValidationResult(failures);

        _mapperMock.Setup(m => m.Map<CustomerValidated>(input)).Returns(customerValidated);
        _validatorMock.Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _mutationResult.Setup(m => m.Error(It.IsAny<CustomerValidated>(), failures, It.IsAny<string>(), HttpStatusCode.BadRequest))
            .Returns(new MutationResponse<CustomerValidated>
            {
                Success = false,
                Data = customerValidated,
                Message = "Validation Error",
                Errors = failures,
                StatusCode = HttpStatusCode.BadRequest
            });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Data.Should().BeEquivalentTo(customerValidated);
        result.Errors.Should().NotBeNull().And.HaveCount(1);

        _validatorMock.Verify(v => v.ValidateAsync(It.IsAny<SoftDeleteCustomerInput>(), It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<CustomerValidated>(It.IsAny<SoftDeleteCustomerInput>()), Times.Once);
        _mutationResult.Verify(m => m.Error(It.IsAny<CustomerValidated>(), It.IsAny<List<ValidationFailure>>(), It.IsAny<string>(), HttpStatusCode.BadRequest), Times.Once);
        _busMock.Verify(p => p.Publish(
            It.IsAny<CustomerValidated>(),
            It.IsAny<CancellationToken>()), Times.Never);
        _mutationResult.Verify(m => m.Ok(It.IsAny<CustomerValidated>(), It.IsAny<string>()), Times.Never);
    }
}
