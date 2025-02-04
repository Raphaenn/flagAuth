using App.IRepository;
using App.Users.Commands;
using Domain.Entities;
using MediatR;

namespace App.Users.CommandsHandlers;

public class CreateFriendshipCmdHandler : IRequestHandler<CreateFriendshipCmd, Guid>
{
    private readonly IFriendRepository _friendRepository;

    public CreateFriendshipCmdHandler(IFriendRepository friendRepository)
    {
        _friendRepository = friendRepository;
    }

    public async Task<Guid> Handle(CreateFriendshipCmd request, CancellationToken cancellationToken)
    {
        Friends friendship = new Friends(request.UserId01, request.UserId02, request.Type);
        Guid response = await _friendRepository.AddFriend(friendship);
        return response;
    }
}