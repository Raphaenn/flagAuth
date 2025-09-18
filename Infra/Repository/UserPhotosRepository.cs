using App.IRepository;
using Domain.Entities;
using Infra.Mappers;
using Infra.Models;
using Microsoft.EntityFrameworkCore;

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
        UserPhotoModel photos = UserPhotoMapper.ToModel(userPhotos);
        await _infraDbContext.UserPhotos.AddAsync(photos);
        await _infraDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<UserPhotos>> GetUserPhotos(Guid userId)
    {
        List<UserPhotoModel> userPhotos = await _infraDbContext.UserPhotos
            .Where(u => u.UserId == userId)
            .ToListAsync();

        List<UserPhotos> result = new List<UserPhotos>();
        foreach (var photos in userPhotos)
        {
            UserPhotos res = UserPhotoMapper.ToDomain(photos);
            result.Add(res);
        }
        return result;
    }
}