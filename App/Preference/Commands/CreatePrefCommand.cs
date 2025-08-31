using Domain.Entities;
using MediatR;

namespace App.Preference.Commands;

public record struct CreatePrefCommand(Guid UserId, string Location, double DistanceKm, string Orientation, int MinAge, int MaxAge, double MinHeight, double MaxHeight, double MinWeight, double MaxWeight) : IRequest<Preferences>;