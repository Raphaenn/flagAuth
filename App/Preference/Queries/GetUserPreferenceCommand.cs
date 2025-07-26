using Domain.Entities;
using MediatR;

namespace App.Preference.Queries;

public record struct GetUserPreferenceCommand(Guid UserId) : IRequest<Preferences>;