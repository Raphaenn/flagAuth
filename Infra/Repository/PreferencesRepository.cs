using App.IRepository;
using Domain.Entities;
using Infra.Mappers;
using Infra.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository;

public class PreferencesRepository : IPreferencesRepository
{
    private readonly InfraDbContext _infraDbContext;

    public PreferencesRepository(InfraDbContext infraDbContext)
    {
        _infraDbContext = infraDbContext;
    }

    public async Task CreatePreferences(Preferences preference)
    {
        PreferencesModel parsedPref =  PreferenceMapper.ToModel(preference);
        await _infraDbContext.preferences.AddAsync(parsedPref);
        await _infraDbContext.SaveChangesAsync();
    }

    public async Task<Preferences?> GetUserPreferences(Guid userId)
    {
        if (_infraDbContext.preferences != null)
        {
            PreferencesModel? pref = await _infraDbContext.preferences.Where(p => p.UserId == userId).FirstOrDefaultAsync();
            if (pref == null)
                return null;
            
            Preferences parsedResult = PreferenceMapper.ToDomain(pref);
            return parsedResult;
        }
        return null;
    }
}