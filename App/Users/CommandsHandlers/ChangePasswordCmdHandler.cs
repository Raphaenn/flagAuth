using App.IRepository;
using App.Users.Commands;
using Domain.Entities;
using MediatR;

namespace App.Users.CommandsHandlers;

public class ChangePasswordCmdHandler : IRequestHandler<ChangePasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;
    
    public ChangePasswordCmdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        User user = await _userRepository.GetUserById(request.UserId.ToString());

        user.ChangePassword(request.OldPassword, request.NewPassword);
        await _userRepository.UpdateUser(user);
        
        return true;
    }
}