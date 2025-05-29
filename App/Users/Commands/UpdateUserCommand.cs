using Domain.Entities;
using MediatR;

namespace App.Users.Commands;

    public record struct UpdateUserCommand(string Id, string Name, string BrithDate, string Country, string City, Sexualities Sexuality, SexualOrientations SexualOrientation, string Password) : IRequest<User>;