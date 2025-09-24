using App.IRepository;
using App.Users.Commands;
using MediatR;

namespace App.Users.CommandsHandlers;

public class UpdateLocationCmdHandler : IRequestHandler<UpdateUserLocationCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public UpdateLocationCmdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<bool> Handle(UpdateUserLocationCommand req, CancellationToken cancellationToken)
    {
        if (req.UserId == null || req.Location == null)
        {
            throw new ArgumentException("Invalid req data");
        }
        await _userRepository.UpdateLocation(req.UserId, req.Location);
        return true;
    }
}