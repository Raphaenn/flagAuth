using Domain.Entities;
using MediatR;

namespace App.Users.Commands;

public record struct CreateUserCommand(string Email) : IRequest<User>;