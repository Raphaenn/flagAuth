using App.IRepository;
using App.Preference.Queries;
using Domain.Entities;
using MediatR;

namespace App.Preference.QueryHandlers;

public class GetUserPreferenceCmdHandler : IRequestHandler<GetUserPreferenceCommand, Preferences?>
{
    private readonly IPreferencesRepository _preferencesRepository;

    public GetUserPreferenceCmdHandler(IPreferencesRepository preferencesRepository)
    {
        _preferencesRepository = preferencesRepository;
    }

    public async Task<Preferences?> Handle(GetUserPreferenceCommand request, CancellationToken cancellationToken)
    {
        Preferences? userPref = await  _preferencesRepository.GetUserPreferences(request.UserId);
        return userPref;
    }
}