using App.IRepository;
using Domain.Entities;
using Infra.Mappers;
using Infra.Models;

namespace Infra.Repository;

public class UserPhotosRepository : IUserPhotoRepository
{
    private readonly InfraDbContext _infraDbContext;

    public UserPhotosRepository(InfraDbContext infraDbContext)
    {
        _infraDbContext = infraDbContext;
    }
    
    public async Task<bool> UploadPhoto(UserPhotos userPhotos)
    {
        try
        {
            UserPhotoModel photos = UserPhotoMapper.ToModel(userPhotos);
            await _infraDbContext.userPhotos.AddAsync(photos);
            await _infraDbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}