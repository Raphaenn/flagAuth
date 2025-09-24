using Domain.Entities;
using MediatR;

namespace App.Users.Commands;

public record struct UpdateUserLocationCommand(string UserId, string Location) : IRequest<bool>;