using App.IRepository;
using App.Users.Commands;
using Domain.Entities;
using MediatR;

namespace App.Users.CommandsHandlers;

public class ChangeUserStatusCmdHandler : IRequestHandler<ChangeUserStatusCommand, User>
{
    private readonly IUserRepository _userRepository;

    public ChangeUserStatusCmdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<User> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
    {
        User user = await _userRepository.GetUserById(request.UserId.ToString());

        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.ChangeStatus(request.Status);
        await _userRepository.ChangeUserStatus(user);
        return user;
    }
}