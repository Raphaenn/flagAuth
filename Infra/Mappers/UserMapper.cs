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
            SexualOrientation = domain.SexualOrientation.ToString(),
            Password = domain.Password
        };
    }

    public static User ToDomain(UserView entity)
    {
        var user = User.Rehydrate(id: entity.Id,
            email: entity.Email,
            name: entity.Name,
            birthdate: entity.Birthdate,
            country: entity.Country,
            city: entity.City,
            sexuality: Enum.TryParse<Sexualities>(entity.Sexuality, out var sex) ? sex : null,
            sexualOrientation: Enum.TryParse<SexualOrientations>(entity.SexualOrientation, out var orientation) ? orientation : null,
            password: entity.Password,
            height: entity.Height,
            weight: entity.Weight,
            latitude: entity.Latitude,
            longitude: entity.Longitude
            );

        // Access private setters via reflection or use internal constructor if available
        typeof(User).GetProperty(nameof(User.Id))?.SetValue(user, entity.Id);
        typeof(User).GetProperty(nameof(User.Birthdate))?.SetValue(user, entity.Birthdate);
        typeof(User).GetProperty(nameof(User.Country))?.SetValue(user, entity.Country);
        typeof(User).GetProperty(nameof(User.City))?.SetValue(user, entity.City);
        typeof(User).GetProperty(nameof(User.Sexuality))?.SetValue(user, entity.Sexuality);
        typeof(User).GetProperty(nameof(User.SexualOrientation))?.SetValue(user, entity.SexualOrientation);
        typeof(User).GetProperty(nameof(User.Password))?.SetValue(user, entity.Password);

        return user;
    }
}