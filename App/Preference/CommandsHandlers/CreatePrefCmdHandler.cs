using App.IRepository;
using App.Preference.Commands;
using Domain.Entities;
using MediatR;

namespace App.Preference.CommandsHandlers;

public class CreatePrefCmdHandler : IRequestHandler<CreatePrefCommand, Preferences>
{
    private readonly IPreferencesRepository _preferencesRepository;

    public CreatePrefCmdHandler(IPreferencesRepository preferencesRepository)
    {
        _preferencesRepository = preferencesRepository;
    }
    
    public async Task<Preferences> Handle(CreatePrefCommand request, CancellationToken cancellationToken)
    {
        Gender parsed = Enum.Parse<Gender>(request.Orientation);
        Console.WriteLine(parsed);
        Preferences pref = Preferences.CreatePref(request.UserId, request.Location, request.DistanceKm, parsed, request.MinAge, request.MaxAge, request.MinHeight, request.MaxHeight, request.MinWeight, request.MaxWeight);
        ;
        await _preferencesRepository.CreatePreferences(pref);
        return pref;
    }
}