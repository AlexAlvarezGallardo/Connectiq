namespace Connectiq.API.Application.Customer.Commands;

public record CreateCustomerCommand(CreateCustomerInput Input) : IRequest<IMutationResponse<CustomerValidated>>;

public class CreateCustomerCommandHandler(
    IPublishEndpoint _publisher,
    IValidator<CustomerValidated> _validator,
    IMutationResultFactory _responseFactory,
    IMapper _mapper) : IRequestHandler<CreateCustomerCommand, IMutationResponse<CustomerValidated>>
{
    public async Task<IMutationResponse<CustomerValidated>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerValidated = _mapper.Map<CustomerValidated>(request.Input);

        var validatorResult = await _validator.ValidateAsync(customerValidated, cancellationToken);

        if (!validatorResult.IsValid)
        {
            var invalidCustomer = customerValidated with { IsValid = false };
            return _responseFactory.Error(invalidCustomer, validatorResult.Errors, "Validation Error");
        }

        await _publisher.Publish(customerValidated, cancellationToken);
        return _responseFactory.Ok(customerValidated);
    }
}

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(c => c.Input.Details)
            .SetValidator(new CustomerDetailsValidator())
            .WithMessage("Los datos del cliente son inválidos.");
    }
}