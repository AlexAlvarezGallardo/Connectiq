using CustomerWorker.Domain.Commands.UpdateCustomerCommand;

namespace CustomerWorker.Events;

public class UpdateCustomerEvent(
    ILogger<UpdateCustomerEvent> _logger,
    IValidator<UpdateCustomer> _validator,
    IMapper _mapper,
    IRepository<CustomerEntity> _repository) : IConsumer<CustomerValidated>
{
    public async Task Consume(ConsumeContext<CustomerValidated> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received UpdateCustomerEvent with EventId: {EventId}", context.MessageId);

        var updateCustomer = _mapper.Map<UpdateCustomer>(message) with
        {
            EventId = context.MessageId?.ToString() ?? Guid.NewGuid().ToString(),
            IsActive = true
        };

        var validationResult = await _validator.ValidateAsync(updateCustomer);

        if(!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for UpdateCustomer with Id: {CustomerId}. Errors: {Errors}", 
                updateCustomer.CustomerValidated.Customer.Id, 
                validationResult.Errors);
            
            throw new ValidationException(validationResult.Errors);
        }

        var customerEntity = _mapper.Map<CustomerEntity>(updateCustomer);

        await _repository.UpdateAsync(customerEntity);
        _logger.LogInformation("Customer updated with Id: {CustomerId}", customerEntity.Id);
    }
}
