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
            Password = domain.Password,
            Status = domain.Status
        };
    }

    public static User ToDomain(UserView entity)
    {
        var user = User.Rehydrate(
            id: entity.Id,
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
            longitude: entity.Longitude,
            status: entity.Status
            );

        // Access private setters via reflection or use internal constructor if available
        // typeof(User).GetProperty(nameof(User.Id))?.SetValue(user, entity.Id);
        return user;
    }
}