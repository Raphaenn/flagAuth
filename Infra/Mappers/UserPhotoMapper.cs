using Domain.Entities;
using Infra.Models;

namespace Infra.Mappers;

public static class UserPhotoMapper
{
    public static UserPhotos ToDomain(UserPhotoModel userPhoto)
    {
        return UserPhotos.Rehydrate(userPhoto.Id, userPhoto.UserId, userPhoto.Url, userPhoto.IsProfilePicture, userPhoto.CreatedAt);
    }

    public static UserPhotoModel ToModel(UserPhotos userPhotos)
    {
        UserPhotoModel data = new UserPhotoModel
        {
            Id = userPhotos.Id,
            UserId = userPhotos.UserId,
            Url = userPhotos.Url,
            IsProfilePicture = userPhotos.IsProfilePicture,
            CreatedAt = userPhotos.UploadedAt
        };

        return data;
    }
}