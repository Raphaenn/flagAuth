using System.Security.Claims;
using Api.Dto;
using App.Preference.Commands;
using Domain.Entities;
using MediatR;

namespace Api.Abstractions.EndpointsDefinitions;

public class PreferencesEndpointDef : IEndpointsDefinitions
{
    public void RegisterEndpoints(WebApplication app)
    {
        app.MapPost("/user/preference/save", async (HttpContext context, IMediator mediator) =>
        {
            var request = await context.Request.ReadFromJsonAsync<CreatePrefRequest>();
            string userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Guid parsedId = Guid.Parse(userId);

            CreatePrefCommand cmd = new CreatePrefCommand(parsedId, request.Location, request.DistanceKm, request.Orientation, request.MinAge, request.MaxAge, request.MinHeight, request.MaxHeight, request.MinWeight, request.MaxWeight);
            Preferences res = await mediator.Send(cmd);;
            return Results.Ok(res);

        }).RequireAuthorization();
    }
}