using AutoMapper;
using Connectiq.Contracts.Interfaces;
using Connectiq.Contracts.User;
using MassTransit;

namespace UserWorker.Events;

public class UserCreatedEvent(
    ILogger<UserCreatedEvent> _logger,
    IMapper _mapper,
    IRepository<UserEntity> _repository) : IConsumer<UserValidated>
{
    public async Task Consume(ConsumeContext<UserValidated> context)
    {
        var message = context.Message;

        var user = _mapper.Map<CreateUser>(message);
        
        var userEntity = _mapper.Map<UserEntity>(user);
        _logger.LogInformation("User Created Event: {@User}", userEntity);

        await _repository.InsertAsync(userEntity);
    }
}
