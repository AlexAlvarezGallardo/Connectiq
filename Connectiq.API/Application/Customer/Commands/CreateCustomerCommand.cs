using AutoMapper;
using Connectiq.Contracts.Customer;
using FluentValidation;
using MassTransit;
using MediatR;

namespace Connectiq.API.Application.Customer.Commands;

public record CreateCustomerCommand(CreateCustomerInput Input) : IRequest<bool>;

public class CreateCustomerCommandHandler(
    IPublishEndpoint _publisher,
    IValidator<CustomerCreated> _validator,
    IMapper _mapper) : IRequestHandler<CreateCustomerCommand, bool>
{
    public async Task<bool> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerCreated = _mapper.Map<CustomerCreated>(request.Input);
        var validatorResult = await _validator.ValidateAsync(customerCreated, cancellationToken);

        if (!validatorResult.IsValid)
            return false;

        await _publisher.Publish(customerCreated, cancellationToken);

        return true;
    }
}

