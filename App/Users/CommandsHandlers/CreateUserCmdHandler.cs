using App.Dto;
using App.IRepository;
using App.Users.Commands;
using App.Users.Integration;
using Domain.Entities;
using MediatR;

namespace App.Users.CommandsHandlers;

public class CreateUserCmdHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _uow;
    private readonly IIntegrationEventPublisher _publisher;

    public CreateUserCmdHandler(IUserRepository userRepository, IUnitOfWork uow, IIntegrationEventPublisher publisher)
    {
        _userRepository = userRepository;
        _uow = uow;
        _publisher = publisher;
    }
    
    public async Task<User> Handle(CreateUserCommand createUserCommand, CancellationToken cancellationToken)
    {
        User user = User.Create(email: createUserCommand.Email, null, null, null, null, null, null, null, null,null,null,null, UserStatus.Inactive, null);
        
        await _userRepository.CreateUser(user);
        await _publisher.PublishAsync(new UserCreatedIntegrationEvent(user.Id, user.Email));
        await _uow.SaveChangesAsync(cancellationToken);
        return user;
    }
}