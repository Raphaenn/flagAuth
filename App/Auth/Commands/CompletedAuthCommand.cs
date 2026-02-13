using Domain.Entities;
using MediatR;

namespace App.Auth.Commands;

public record struct CompletedAuthCommand(User User): IRequest<string>;