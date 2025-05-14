using App.IRepository;
using App.Users.Commands;
using Domain.Entities;
using MediatR;

namespace App.Users.CommandsHandlers;

public class GetUserByEmailCmdHandler : IRequestHandler<GetUserByEmailCommand, User>
{
    private readonly IUserRepository _userRepository;

    public GetUserByEmailCmdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle(GetUserByEmailCommand request, CancellationToken cancellationToken)
    {
        throw new ApplicationException();
    }
}