using Connectiq.ProjectDefaults.EventBus;
using Connectiq.ProjectDefaults.Response.Mutation.Factory;
using Microsoft.Extensions.Options;

namespace Connectiq.API.Application.Customer.Commands;

public record UpdateCustomerCommand(UpdateCustomerInput Input) : IRequest<IMutationResponse<CustomerValidated>>;

public class UpdateCustomerCommandHandler(
    IBus _bus,
    IValidator<UpdateCustomerInput> _validator,
    IMutationResultFactory _responseFactory,
    IMapper _mapper,
    IOptions<EventBusOptions> _options) : IRequestHandler<UpdateCustomerCommand, IMutationResponse<CustomerValidated>>
{
    public async Task<IMutationResponse<CustomerValidated>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
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
            ctx.SetRoutingKey(_options.Value.Exchange.UpdateCustomer.RoutingKey);
        }, cancellationToken);

        return _responseFactory.Ok(customerValidated);
    }
}

public class UpdateCustomerInputCommandValidator : AbstractValidator<UpdateCustomerInput>
{
    public UpdateCustomerInputCommandValidator()
    {
        RuleFor(c => c.Customer.Details)
            .SetValidator(new CustomerDetailsValidator());

        RuleFor(c => c.Customer.Id)
            .NotEmpty()
            .WithMessage("El campo Id es obligatorio.");
    }
}