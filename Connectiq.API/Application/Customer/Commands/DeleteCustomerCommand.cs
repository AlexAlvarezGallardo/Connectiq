using Connectiq.ProjectDefaults.EventBus;
using Microsoft.Extensions.Options;

namespace Connectiq.API.Application.Customer.Commands;

public record DeleteCustomerCommand(DeleteCustomerInput Input) : IRequest<IMutationResponse<CustomerValidated>>;

public class DeleteCustomerCommandHandler(
    IPublishEndpoint _publishEndpoint,
    IValidator<DeleteCustomerInput> _validator,
    IMutationResultFactory _responseFactory,
    IMapper _mapper,
    IOptions<EventBusOptions> _options) : IRequestHandler<DeleteCustomerCommand, IMutationResponse<CustomerValidated>>
{
    public async Task<IMutationResponse<CustomerValidated>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await _validator.ValidateAsync(request.Input, cancellationToken);

        var customerValidated = _mapper.Map<CustomerValidated>(request.Input);

        if (!validatorResult.IsValid)
        {
            var invalidCustomer = customerValidated with { IsValid = false };
            return _responseFactory.Error(invalidCustomer, validatorResult.Errors, "Validation Error");
        }

        await _publishEndpoint.Publish(customerValidated, ctx =>
        {
            ctx.SetRoutingKey(_options.Value.Exchange.DeleteCustomer.RoutingKey);
        }, cancellationToken);

        return _responseFactory.Ok(customerValidated);
    }
}

public class DeleteCustomerInputCommandValidator : AbstractValidator<DeleteCustomerInput>
{
    public DeleteCustomerInputCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("El Id del customer es obligatorio");
    }
}