using Connectiq.ProjectDefaults.EventBus;
using Microsoft.Extensions.Options;

namespace Connectiq.API.Application.Customer.Commands;

public record CreateCustomerCommand(CreateCustomerInput Input) : IRequest<IMutationResponse<CustomerValidated>>;

public class CreateCustomerCommandHandler(
    IBus _bus,
    IValidator<CreateCustomerInput> _validator,
    IMutationResultFactory _responseFactory,
    IMapper _mapper,
    IOptions<EventBusOptions> _options) : IRequestHandler<CreateCustomerCommand, IMutationResponse<CustomerValidated>>
{
    public async Task<IMutationResponse<CustomerValidated>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await _validator.ValidateAsync(request.Input, cancellationToken);

        var customerValidated = _mapper.Map<CustomerValidated>(request.Input);

        if (!validatorResult.IsValid)
        {
            var invalidCustomer = customerValidated with { IsValid = false };
            return _responseFactory.Error(invalidCustomer, validatorResult.Errors, "Validation Error");
        }

        await _bus.Publish(customerValidated, ctx =>
        {
            ctx.SetRoutingKey(_options.Value.Exchange.CreateCustomer.RoutingKey);
        }, cancellationToken);

        return _responseFactory.Ok(customerValidated);
    }
}

public class CreateCustomerInputCommandValidator : AbstractValidator<CreateCustomerInput>
{
    public CreateCustomerInputCommandValidator()
    {
        RuleFor(c => c.Details)
            .SetValidator(new CustomerDetailsValidator())
            .WithMessage("Los datos del cliente son inválidos.");
    }
}