using AutoMapper;
using Connectiq.API.Application.Customer.Commands;
using Connectiq.ProjectDefaults.Response.Factory;
using Connectiq.ProjectDefaults.Response.Factory.Mutation;
using Connectiq.Tests.Utilities;
using Customers.Commands;
using CustomerWorker.Domain.Commands;
using FluentAssertions;
using MassTransit;
using Moq;

namespace Connectiq.API.UnitTests.Application.Customer.Commands;

public class CreateCustomerCommandHandlerTests
{
    readonly Mock<IPublishEndpoint> _publisherMock = new();
    readonly Mock<IMapper> _mapperMock = new();
    readonly Mock<IMutationResultFactory> _mutationResult = new();
    readonly CreateCustomerCommandHandler _handler;
    readonly CreateCustomerCommandValidator _validator = new();

    readonly string _basePath = "Customers/Commands";

    public CreateCustomerCommandHandlerTests()
    {
        _handler = new CreateCustomerCommandHandler(
            _publisherMock.Object,
            _mutationResult.Object,
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
        _publisherMock.Setup(p => p.Publish(customerValidated, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mutationResult.Setup(f => f.Ok(It.IsAny<CustomerValidated>(), It.IsAny<string>()))
            .Returns(new MutationResponse<CustomerValidated> { Success = true, Data = customerValidated, Message = string.Empty });
        
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(customerValidated);
        _publisherMock.Verify(p => p.Publish(customerValidated, It.IsAny<CancellationToken>()), Times.Once);
    }
}