using System.Security.Cryptography;
using App.Users.Queries;
using MediatR;
using Api.Extensions;
using App.Auth.Commands;
using Domain.Entities;

namespace Api.Abstractions.EndpointsDefinitions;

public class AuthEndpointDef : IEndpointsDefinitions
{
    private record Result(User User, string Token);
    
    public void RegisterEndpoints(WebApplication app)
    {
        app.MapGet("/auth/verify", async (HttpContext context, IMediator mediator) =>
        {
            // Get social token from request header
            string token = context.Request.Headers["authorization"];
            if (string.IsNullOrEmpty(token))
            {
                return Results.Unauthorized();
            }
            token = token.Substring("Bearer ".Length).Trim();

            try
            {
                RSA publicKey = PublicKeyLoader.LoadPublicKey();
                var validatedClaims = JwtTokenValidator.ValidateJwtToken(token, publicKey);

                // Extract the email claim (assuming it's named "email")
                var emailClaim = validatedClaims.FindFirst("emailAddress")?.Value;

                if (emailClaim == null)
                {
                    return Results.BadRequest("Invalid token data");
                }
                
                // Check user data
                GetUserQuery userQuery = new GetUserQuery
                {
                    Email = emailClaim
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
           
        });

        app.MapPost("auth/create", async () =>
        {
            return Results.Ok();
        });
    }
}