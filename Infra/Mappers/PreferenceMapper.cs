using Domain.Entities;
using Infra.Models;

namespace Infra.Mappers;

public static class PreferenceMapper
{
    public static Preferences ToDomain(PreferencesModel preference)
    {
        return Preferences.Rehydrate(
            preference.Id,
            preference.UserId,
            preference.Location,
            preference.DistanceKm,
            preference.GenderPreference,
            preference.MinAge,
            preference.MaxAge,
            preference.MinHeight,
            preference.MaxHeight,
            preference.MinWeight,
            preference.MaxWeight
        );
    }

    public static PreferencesModel ToModel(Preferences preference)
    {
        return new PreferencesModel
        {
            Id = preference.Id,
            UserId = preference.UserId,
            Location = preference.Location,
            DistanceKm = preference.DistanceKm,
            GenderPreference = preference.GenderPreference,
            MinAge = preference.MinAge,
            MaxAge = preference.MaxAge,
            MinHeight = preference.MinHeight,
            MaxHeight = preference.MaxHeight,
            MinWeight = preference.MinWeight,
            MaxWeight = preference.MaxWeight,
            Interests = null
        };
    }
}