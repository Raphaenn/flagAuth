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
        Console.WriteLine("Foi aqui");
        var user = User.Rehydrate(id: entity.Id,
            email: entity.Email,
            name: entity.Name,
            birthdate: entity.Birthdate,
            country: entity.Country,
            city: entity.City,
            sexuality: Enum.TryParse<Sexualities>(entity.Sexuality, out var sex) ? sex : (Sexualities?)null,
            sexualOrientation: Enum.TryParse<SexualOrientations>(entity.SexualOrientation, out var orientation) ? orientation : (SexualOrientations?)null,
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
        typeof(User).GetProperty(nameof(User.Sexuality))?.SetValue(user, sex);
        typeof(User).GetProperty(nameof(User.SexualOrientation))?.SetValue(user, orientation);
        typeof(User).GetProperty(nameof(User.Password))?.SetValue(user, entity.Password);
        typeof(User).GetProperty(nameof(User.Height))?.SetValue(user, entity.Height);
        typeof(User).GetProperty(nameof(User.Weight))?.SetValue(user, entity.Weight);
        typeof(User).GetProperty(nameof(User.Latitude))?.SetValue(user, entity.Latitude);
        typeof(User).GetProperty(nameof(User.Longitude))?.SetValue(user, entity.Longitude);

        return user;
    }
}