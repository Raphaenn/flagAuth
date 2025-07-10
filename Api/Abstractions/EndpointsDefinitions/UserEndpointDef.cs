using App.Users.Commands;
using App.Users.DTOs;
using App.Users.Queries;
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
            CreateUserCommand userCmd = new CreateUserCommand(request.Email);
            User response = await mediator.Send(userCmd);
            return Results.Ok(response);
        }).AllowAnonymous();

        app.MapGet("/users/refresh/info", async (HttpContext context, IMediator mediator) =>
        {
            var request = await context.Request.ReadFromJsonAsync<CreateUserRequest>();
            GetUserByIdQuery userQuery = new GetUserByIdQuery(request.Id);
            User response = await mediator.Send(userQuery);
            return response;
        });
    }
}