using Domain.Entities;
using MediatR;

namespace App.Users.Commands;

public record struct UploadUserPhotosCommand(string UserId, string Url, bool IsProfile) : IRequest<UserPhotos>;
