using Domain.Entities;
using MediatR;

namespace App.Users.Commands;

public record struct CreateFriendshipCmd(Guid UserId01, Guid UserId02, FriendShipType Type) : IRequest<Guid>;