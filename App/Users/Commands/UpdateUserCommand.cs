using Domain.Entities;
using MediatR;

namespace App.Users.Commands;

    public record struct UpdateUserCommand(string Id, string Name, string BirthDate, string Country, string City, Sexualities Sexuality, SexualOrientations SexualOrientation, string Password, double? Height, double? Weight, double? Latitude, double? Longitude) : IRequest<User>;