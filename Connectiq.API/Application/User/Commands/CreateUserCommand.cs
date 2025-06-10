using AutoMapper;
using Connectiq.Contracts.User;
using FluentValidation;
using MassTransit;
using MediatR;

namespace Connectiq.API.Application.User.Commands;

public record CreateUserCommand(CreateUserInput UserInput) : IRequest<bool>;

public class CreateUserCommandHandler(
    IPublishEndpoint _publisher,
    IValidator<CreateUserInput> _validator,
    IMapper _mapper) : IRequestHandler<CreateUserCommand, bool>
{
    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request.UserInput, cancellationToken);

        if (!validationResult.IsValid) 
        {
            var notValidatedUser = _mapper.Map<UserNotValidated>(request.UserInput) with
            {
                NotValidatedMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
            };
            return false;
        }

        var userValidated = _mapper.Map<UserValidated>(request.UserInput);
        await _publisher.Publish(userValidated, cancellationToken);

        return true;
    }
}