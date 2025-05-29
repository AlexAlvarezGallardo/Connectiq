using AutoMapper;
using Connectiq.Contracts.Customer;
using Connectiq.Tests.Utilities;
using Connectiq.Tests.Utilities.Fixtures;
using FluentAssertions;
using MassTransit;
using Moq;
using PersistenceWorker.Consumers.Customers;
using PersistenceWorker.Repository;

namespace Connectiq.PersistenceWorker.Tests.Consumers.Customers;

public class CustomerValidatedEventTests : IClassFixture<MapperFixture>
{
    readonly Mock<IRepository<CustomerEntity>> _repositoryMock = new();
    readonly Mock<ConsumeContext<CustomerValidated>> _contextMock = new();
    readonly IMapper _mapper;

    public CustomerValidatedEventTests(MapperFixture fixture)
    {
        _mapper = fixture.Mapper;
    }

    [Fact]
    public async Task Consume_Should_Map_And_Insert_CustomerEntity()
    {
        var jsonPath = JsonDataLoader.GetDataPath("CustomerValidated.json");
        var customerValidated = JsonDataLoader.LoadFromFile<CustomerValidated>(jsonPath);

        var expectedEntity = _mapper.Map<CustomerEntity>(customerValidated);

        _contextMock.SetupGet(x => x.Message).Returns(customerValidated);
        _contextMock.SetupGet(x => x.MessageId).Returns(Guid.NewGuid());

        CustomerEntity capturedEntity = null!;
        _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<CustomerEntity>()))
            .Callback<CustomerEntity>(entity => capturedEntity = entity)
            .ReturnsAsync(true);

        var sut = new CustomerValidatedEvent(_repositoryMock.Object, _mapper);

        await sut.Consume(_contextMock.Object);

        _repositoryMock.Verify(r => r.InsertAsync(It.IsAny<CustomerEntity>()), Times.Once);

        capturedEntity
            .Should()
            .BeEquivalentTo(expectedEntity, options => options
            .Excluding(e => e.Id));
    }
}