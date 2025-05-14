using App.Users.Commands;
using App.Users.DTOs;
using Domain.Entities;
using MediatR;

namespace Api.Abstractions.EndpointsDefinitions;

public class UserEndpointDef : IEndpointsDefinitions
{
    public void RegisterEndpoints(WebApplication app)
    {
        app.MapPost("/users/create", async (HttpContext context, IMediator mediator) =>
        {
            var request = await context.Request.ReadFromJsonAsync<CreateUserRequest>();
            CreateUserCommand userCmd = new CreateUserCommand(request.Email, request.Name);
            User response = await mediator.Send(userCmd);
            return Results.Ok(response);
        }).AllowAnonymous();

        app.MapGet("/users/check-email", (HttpContext context, IMediator mediator) =>
        {
            
        });
    }
}