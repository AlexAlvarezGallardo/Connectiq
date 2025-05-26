using AutoMapper;
using Connectiq.Contracts.Customer;
using FluentValidation;
using MassTransit;

namespace CustomerWorker.Events;

internal class CustomerCreatedEvent(
    ILogger<CustomerCreatedEvent> _logger,
    IMapper _mapper,
    IValidator<CustomerValidated> _validator,
    IPublishEndpoint _bus) : IConsumer<CustomerCreated>
{
    public async Task Consume(ConsumeContext<CustomerCreated> context)
    {
        var message = context.Message;
        var validatedCustomer = _mapper.Map<CustomerValidated>(message) with
        {
            CustomerCreated = message with
            {
                EventId = context.MessageId?.ToString() ?? Guid.NewGuid().ToString(),
                IsActive = true
            }
        };

        var validationResult = await _validator.ValidateAsync(validatedCustomer);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            _logger.LogError("Customer validation failed: {Errors}", errors);

            var notValidatedCustomer = _mapper.Map<CustomerNotValidated>(message) with
            {
                NotValidatedMessage = errors,
                IsValid = false
            };

            await _bus.Publish(notValidatedCustomer, context.CancellationToken);
            return;
        }

        _logger.LogInformation("Customer received {EventId} - {Name}", validatedCustomer.CustomerCreated.EventId, validatedCustomer.CustomerCreated.CustomerData.Name);
        await _bus.Publish(validatedCustomer, context.CancellationToken);
    }
}
