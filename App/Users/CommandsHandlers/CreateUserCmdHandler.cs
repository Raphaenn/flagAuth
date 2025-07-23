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
        User user = User.Create(email: createUserCommand.Email, null, null, null, null, null, null, null, null,null,null,null, UserStatus.Incomplete);
        await _userRepository.CreateUser(user);
        return user;
    }
}