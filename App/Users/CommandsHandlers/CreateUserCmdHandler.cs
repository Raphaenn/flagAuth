using App.IRepository;
using App.Users.Commands;
using Domain.Entities;
using MediatR;

namespace App.Users.CommandsHandlers;

public class CreateUserCmdHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCmdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<User> Handle(CreateUserCommand createUserCommand, CancellationToken cancellationToken)
    {
        User user = new User(email: createUserCommand.Email, name: createUserCommand.Name);
        await _userRepository.CreateUser(user);
        return user;
    }
}