using Domain.Entities;

namespace App.IRepository;

public interface IPreferencesRepository
{
    Task CreatePreferences(Preferences preference);

    Task<Preferences?> GetUserPreferences(Guid userId);
}