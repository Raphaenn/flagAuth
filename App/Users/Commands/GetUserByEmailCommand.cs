using Domain.Entities;
using MediatR;

namespace App.Users.Commands;

public record struct GetUserByEmailCommand(string Email) : IRequest<User>;