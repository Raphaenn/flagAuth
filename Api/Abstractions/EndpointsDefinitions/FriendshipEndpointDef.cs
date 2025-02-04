using App.Users.Commands;
using App.Users.DTOs;
using Domain.Entities;
using MediatR;

namespace Api.Abstractions.EndpointsDefinitions;

public class FriendshipEndpointDef : IEndpointsDefinitions
{

    public void RegisterEndpoints(WebApplication app)
    {
        app.MapPost("/user/friend/add", async (HttpContext context, IMediator mediator ) =>
        {
            var request = await context.Request.ReadFromJsonAsync<AddFriendRequest>();
            
            if (request is null)
                return Results.BadRequest("Invalid request");    
            
            if (!Enum.TryParse<FriendShipType>(request.Type, true, out var friendshipType))
                return Results.BadRequest("Invalid friendship type");
    
            CreateFriendshipCmd newCommand = new CreateFriendshipCmd
            {
                UserId01 = request.UserId01,
                UserId02 = request.UserId02,
                Type = friendshipType,
            };

            var response = await mediator.Send(newCommand);
            
            return Results.Ok(response);
        });

        
        app.MapPost("/friends/accept", (HttpContext context) =>
        {
            return Results.Ok();
        });
    }
}