using Domain.Entities;
using Infra.Models;

namespace Infra.Mappers;

public static class UserMapper
{
    public static UserView ToEntity(User domain)
    {
        return new UserView
        {
            Id = domain.Id,
            Name = domain.Name,
            Email = domain.Email,
            Birthdate = domain.Birthdate,
            Country = domain.Country,
            City = domain.City,
            Sexuality = domain.Sexuality.ToString(),
            SexualOrientation = domain.SexualOrientation.ToString()
        };
    }

    public static User ToDomain(UserView entity)
    {
        return new User(entity.Email, entity.Name);
    }
}