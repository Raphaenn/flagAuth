using App.IRepository;
using App.Users.Commands;
using Domain.Entities;
using MediatR;

namespace App.Users.CommandsHandlers;

public class CreateUserCmdHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCmdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Guid> Handle(CreateUserCommand createUserCommand, CancellationToken cancellationToken)
    {
        User user = new User(email: createUserCommand.Email, name: createUserCommand.Name);
        Guid response = await _userRepository.CreateUser(user);
        return response;
    }
}