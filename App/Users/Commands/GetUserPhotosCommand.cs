using Domain.Entities;
using MediatR;

namespace App.Users.Commands;

public record struct GetUserPhotosCommand(string UserId) : IRequest<List<UserPhotos>>;