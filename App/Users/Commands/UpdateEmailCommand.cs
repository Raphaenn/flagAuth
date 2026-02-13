using Domain.Entities;
using MediatR;

namespace App.Users.Commands;

public record struct UpdateEmailCommand(Guid UserId, string NewEmail) : IRequest<bool>;