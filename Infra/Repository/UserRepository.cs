using System.Diagnostics;
using App.IRepository;
using Domain.Entities;
using Infra.Mappers;
using Infra.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository;

public class UserRepository : IUserRepository
{
    private readonly InfraDbContext _infraDbContext;

    public UserRepository(InfraDbContext infraDbContext)
    {
        _infraDbContext = infraDbContext;
    }

    public async Task<User?> GetUser(string email)
    {
        if (_infraDbContext.UsersView != null)
        {
            UserDbModel? response = await _infraDbContext.UserWriteModel
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (response != null)
            {
                return User.Rehydrate(id: response.Id,
                    email: response.Email,
                    name: response.Name,
                    birthdate: response.Birthdate,
                    country: response.Country,
                    city: response.City,
                    sexuality: Enum.TryParse<Sexualities>(response.Sexuality, out var sex) ? sex : null,
                    sexualOrientation: Enum.TryParse<SexualOrientations>(response.SexualOrientation, out var orientation) ? orientation : null,
                    password: response.Password,
                    height: response.Height,
                    weight: response.Weight,
                    latitude: response.Latitude,
                    longitude: response.Longitude,
                    status: response.Status
                );
            }
        }
        return null;
    }

    public async Task<Guid> CreateUser(User user)
    {
        var request = UserMapper.ToEntity(user);
        await _infraDbContext.UserWriteModel.AddAsync(request);
        return request.Id;
    }

    public async Task<Boolean> UpdateUser(User user)
    {
        UserDbModel? entity = await _infraDbContext.UserWriteModel!.FindAsync(user.Id);
        if (entity != null)
        {
            entity.Name = user.Name;
            entity.Birthdate = user.Birthdate;
            entity.Country = user.Country;
            entity.City = user.City;
            entity.Sexuality = user.Sexuality.ToString();
            entity.SexualOrientation = user.SexualOrientation.ToString();
            entity.Password = user.Password;
            entity.Height = user.Height;
            entity.Weight = user.Weight;
            entity.Latitude = user.Latitude;
            entity.Longitude = user.Longitude;
            entity.Status = user.Status;
        }

        await _infraDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<User> GetUserById(string id)
    {
        if (!Guid.TryParse(id, out Guid guidId))
            throw new ArgumentException("Invalid id");

        UserDbModel? response = await _infraDbContext.UserWriteModel.FirstOrDefaultAsync(u => u.Id == guidId);
        if (response == null)
        {
            throw new Exception("User not found");
        }

        User user = UserMapper.ToDomain(response);

        return user;
    }

    public async Task ChangeUserStatus(User user)
    {
        if (_infraDbContext.UserWriteModel != null)
        {
            UserDbModel parsedUser = UserMapper.ToEntity(user);
            _infraDbContext.UserWriteModel.Update(parsedUser);
            await _infraDbContext.SaveChangesAsync();
            return;
        }
        return;
    }

    public async Task UpdateLocation(string id, string location)
    {
        if (!Guid.TryParse(id, out Guid guidId))
            throw new ArgumentException("Invalid id");

        if (_infraDbContext.UserWriteModel != null)
        {
            var user = await _infraDbContext.UserWriteModel.FindAsync(guidId);
            if (user is null) return;

            user.City = location;
        }

        await _infraDbContext.SaveChangesAsync();
    }
    
}