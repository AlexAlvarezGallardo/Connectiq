using Connectiq.API.Application.User.Commands;
using Connectiq.Contracts.User;
using MediatR;

namespace Connectiq.API.GraphQL.User;

public class UserMutation
{
    public async Task<bool> CreateUser(CreateUserInput input, [Service] ISender sender)
    {
        var command = new CreateUserCommand(input);
        return await sender.Send(command);
    }
}