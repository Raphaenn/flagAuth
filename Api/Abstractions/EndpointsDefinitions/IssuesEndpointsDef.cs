using App.Issue.Commands;
using App.Users.Commands;
using App.Users.DTOs;
using Domain.Entities;
using MediatR;

namespace Api.Abstractions.EndpointsDefinitions;

public class IssuesEndpointsDef : IEndpointsDefinitions
{
    private record struct IssueCreateReq(Guid UserId, string Content);

    public void RegisterEndpoints(WebApplication app)
    {
        app.MapPost("/issues/register", async (HttpContext context, IMediator mediator) =>
        {
            var request = await context.Request.ReadFromJsonAsync<IssueCreateReq>();
            CreateIssueCommand userCmd = new CreateIssueCommand(request.UserId, request.Content);
            Issues response = await mediator.Send(userCmd);
            return Results.Ok(response);
        }).AllowAnonymous();
    }
}