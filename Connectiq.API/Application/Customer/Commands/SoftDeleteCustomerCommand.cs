using Connectiq.ProjectDefaults.EventBus;
using Connectiq.ProjectDefaults.Response.Mutation.Factory;
using Microsoft.Extensions.Options;

namespace Connectiq.API.Application.Customer.Commands;

public record SoftDeleteCustomerCommand(SoftDeleteCustomerInput Input) : IRequest<IMutationResponse<CustomerValidated>>;

public class SoftDeleteCustomerCommandHandler(
    IBus _bus,
    IValidator<SoftDeleteCustomerInput> _validator,
    IMutationResultFactory _responseFactory,
    IMapper _mapper,
    IOptions<EventBusOptions> _options) : IRequestHandler<SoftDeleteCustomerCommand, IMutationResponse<CustomerValidated>>
{
    public async Task<IMutationResponse<CustomerValidated>> Handle(SoftDeleteCustomerCommand request, CancellationToken cancellationToken)
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
            ctx.SetRoutingKey(_options.Value.Exchange.SoftDeleteCustomer.RoutingKey);
        }, cancellationToken);

        return _responseFactory.Ok(customerValidated);
    }
}

public class SoftDeleteCustomerInputCommandValidator : AbstractValidator<SoftDeleteCustomerInput>
{
    public SoftDeleteCustomerInputCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("El Id del customer es obligatorio");
    }
}