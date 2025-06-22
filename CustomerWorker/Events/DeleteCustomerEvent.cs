using CustomerWorker.Domain.Commands.DeleteCustomerCommand;

namespace CustomerWorker.Events;

public class DeleteCustomerEvent(
    ILogger<DeleteCustomerEvent> _logger,
    IMapper _mapper,
    IRepository<CustomerEntity> _repository) : IConsumer<CustomerValidated>
{
    public async Task Consume(ConsumeContext<CustomerValidated> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received DeleteCustomerEvent with EventId: {EventId}", context.MessageId);

        var deleteCustomer = _mapper.Map<DeleteCustomer>(message);

        var customerEntity = _mapper.Map<CustomerEntity>(deleteCustomer);

        await _repository.DeleteAsync(customerEntity);
        _logger.LogInformation("Customer delete with Id: {CustomerId}", customerEntity.Id);
    }
}
