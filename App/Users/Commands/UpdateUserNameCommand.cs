using Domain.Entities;
using MediatR;

namespace App.Users.Commands;

public record struct UpdateUserNameCommand(Guid UserId, string NewName) : IRequest<bool>;