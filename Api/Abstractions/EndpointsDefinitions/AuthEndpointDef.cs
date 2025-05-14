using System.Security.Cryptography;
using App.Users.Queries;
using MediatR;
using Api.Extensions;
using App.Auth.Commands;
using App.Auth.DTOs;
using App.Users.Commands;
using Domain.Entities;
using YamlDotNet.Core.Tokens;

namespace Api.Abstractions.EndpointsDefinitions;

public class AuthEndpointDef : IEndpointsDefinitions
{
    private record Result(User User, string Token);
    
    public void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/auth/signin", async (HttpContext context, IMediator mediator) =>
        {
            Console.WriteLine("cai aqui");
            foreach (var us in context.User.Claims)
            {
                Console.WriteLine(us.Value);
            }
            
            // if (!context.User.Identity?.IsAuthenticated ?? false)
            //     return Results.Unauthorized();

            var userMail = context.User.FindFirst("emailAddress")?.Value;

             try
             {
                 // Validate toke with public key
                 // RSA publicKey = PublicKeyLoader.LoadPublicKey();
                 // var validatedClaims = JwtTokenValidator.ValidateJwtToken(token, publicKey);
            
                 if (userMail == null)
                 {
                     return Results.BadRequest("Invalid token data");
                 }
                 
                 // Check user data
                 GetUserQuery userQuery = new GetUserQuery
                 {
                     Email = userMail
                 };
                 
                 User? userData = await mediator.Send(userQuery);
            
                 // Validate if user exists
                 if (userData == null)
                 {
                     return Results.BadRequest("User not found");
                 }
                 
                 // create the real auth token
                 CreateAuthCommand createToken = new CreateAuthCommand
                 {
                     Id = userData.Id,
                     Email = userData.Email,
                     Name = userData.Name
                 };
                 string createdToken = await mediator.Send(createToken);
            
                 Result res = new Result(userData, createdToken);
                 
                 return Results.Ok(res);
             }
             catch (Exception e)
             {
                 return Results.BadRequest(e.Message);
             }
        }).RequireAuthorization();

        app.MapPost("/auth/signup", async (HttpContext context, IMediator mediator) =>
        {
            try
            {
                // get body
                var request = await context.Request.ReadFromJsonAsync<SignUpRequest>();
            
                // search by user mail
                GetUserQuery userQuery = new GetUserQuery
                {
                    Email = request.Email
                };

                User? userData = await mediator.Send(userQuery);

                // return if already exists
                if (userData != null)
                {
                    return Results.BadRequest("User already exists");
                }
            
                // create new user
                CreateUserCommand userCmd = new CreateUserCommand(request.Email, request.Name);
                User response = await mediator.Send(userCmd);
            
                // Create a session token
                CreateAuthCommand createToken = new CreateAuthCommand
                {
                    Id = response.Id,
                    Email = request.Email,
                    Name = request.Name
                };
                string createdToken = await mediator.Send(createToken);
                
                // Anonymous object 
                // var user = new
                // {
                //     id = 123,
                //     name = "Lionel Messi",
                //     email = "messie@gmail.com"
                // };
                //
                // return Results.Created($"/users/{user.id}", new { user });
            
                return Results.Created("/auth/signup", new Result(User: response, Token: createdToken));
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
    }
}