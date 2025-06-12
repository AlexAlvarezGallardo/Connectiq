using AutoMapper;
using CustomerWorker.Domain;
using CustomerWorker.Domain.Commands;
using MassTransit;
using PersistenceWorker.Repository;

namespace PersistenceWorker.Consumers.Customers;

public class CustomerValidatedEvent(
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
