using CustomerWorker.Domain.Commands.SoftDeleteCustomerCommand;

namespace CustomerWorker.Events;

public class SoftDeleteCustomerEvent(
    ILogger<SoftDeleteCustomerEvent> _logger,
    IMapper _mapper,
    IRepository<CustomerEntity> _repository) : IConsumer<CustomerValidated>
{
    public async Task Consume(ConsumeContext<CustomerValidated> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received SoftDeleteCustomerEvent with EventId: {EventId}", context.MessageId);

        var deleteCustomer = _mapper.Map<SoftDeleteCustomer>(message);

        var customerEntity = _mapper.Map<CustomerEntity>(deleteCustomer);

        await _repository.SoftDeleteAsync(customerEntity);
        _logger.LogInformation("Customer soft deleted with Id: {CustomerId}", customerEntity.Id);
    }
}
