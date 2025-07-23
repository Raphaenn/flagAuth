using Domain.Entities;
using MediatR;

namespace App.Users.Commands;

public record struct ChangeUserStatusCommand(Guid UserId, UserStatus Status) : IRequest<User>;