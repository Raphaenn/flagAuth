using Api.Dto;
using App.Users.Commands;
using App.Users.DTOs;
using App.Users.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

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

        app.MapPut("/user/update", async (HttpContext context, IMediator mediator) =>
        {
            try
            {
                var request = await context.Request.ReadFromJsonAsync<CompleteUserRequest>();
                
                if (string.IsNullOrWhiteSpace(request?.Password))
                    return Results.BadRequest("Password cannot be empty");

                var hasher = new PasswordHasher<object>();
                string passwordHash = hasher.HashPassword(null, request.Password);

                UpdateUserCommand completeUser = new UpdateUserCommand
                {
                    Id = request.Id,
                    Name = request.Name,
                    BirthDate = request.BirthDate,
                    Country = request.Country,
                    City = request.City,
                    Sexuality = request.Sexuality,
                    SexualOrientation = request.SexualOrientation,
                    Password = passwordHash,
                    Height = request.Height,
                    Weight = request.Weight,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude
                };

                User user = await mediator.Send(completeUser);

                return Results.Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
        });

    }
}