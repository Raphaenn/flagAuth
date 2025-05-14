using Domain.Entities;
using MediatR;

namespace App.Users.Commands;

public record struct CreateUserCommand(string Email, string Name) : IRequest<User>;