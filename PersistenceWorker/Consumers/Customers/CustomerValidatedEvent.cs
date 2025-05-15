using AutoMapper;
using Connectiq.Contracts.Customer;
using MassTransit;
using PersistenceWorker.Repository;

namespace PersistenceWorker.Consumers.Customers;

internal class CustomerValidatedEvent(
    IRepository<CustomerEntity> _dbRepository,
    IMapper _mapper) : IConsumer<CustomerValidated>
{
    public async Task Consume(ConsumeContext<CustomerValidated> context)
    {
        var message = context.Message;
        var customer = _mapper.Map<CustomerEntity>(message);

        await _dbRepository.InsertAsync(customer);
    }
}
