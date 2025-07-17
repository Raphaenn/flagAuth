using Domain.Entities;

namespace App.IRepository;

public interface IUserPhotoRepository
{
    public Task<Boolean> UploadPhoto(UserPhotos userPhotos);
}