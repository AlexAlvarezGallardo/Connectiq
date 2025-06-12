namespace CustomerWorker.Events;

public class CustomerCreatedEvent(
    ILogger<CustomerCreatedEvent> _logger,
    IMapper _mapper,
    IRepository<CustomerEntity> _repository) : IConsumer<CustomerValidated>
{
    public async Task Consume(ConsumeContext<CustomerValidated> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received CustomerCreatedEvent with EventId: {EventId}", message.Customer.Name);

        var customerCreate = _mapper.Map<CustomerCreate>(message) with
        {
            EventId = context.MessageId?.ToString() ?? Guid.NewGuid().ToString(),
            IsActive = true
        };

        var customerEntity = _mapper.Map<CustomerEntity>(customerCreate);

        await _repository.InsertAsync(customerEntity);
        _logger.LogInformation("Customer created with Id: {CustomerId}", customerEntity.Id);
    }
}
