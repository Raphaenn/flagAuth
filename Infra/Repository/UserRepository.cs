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
        if (_infraDbContext.users_view != null)
        {
            UserView? response = await _infraDbContext.users_view
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

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
                    longitude: response.Longitude
                );
            }
        }
        return null;
    }

    public async Task<Guid> CreateUser(User user)
    {
        try
        {
            var request = UserMapper.ToEntity(user);
            await _infraDbContext.users.AddAsync(request);
            await _infraDbContext.SaveChangesAsync();
            return user.Id;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<Boolean> UpdateUser(User user)
    {
        try
        {
            var entity = await _infraDbContext.users!.FindAsync(user.Id);
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
            }

            await _infraDbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            throw new Exception($"Erro ao tentar atualizar usuário: {e.Message}");
        }
    }

    public async Task<User> GetUserById(string id)
    {
        try
        {
            if (!Guid.TryParse(id, out Guid guidId))
                throw new ArgumentException("Invalid id");

            UserView? response = await _infraDbContext.users_view!.FindAsync(guidId);
            if (response == null)
            {
                throw new Exception("User not found");
            }

            User user = UserMapper.ToDomain(response);

            return user;

        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}